using System.Diagnostics;
using System.Text;
using System.Text.Json;
using AuthyDecryptor.Model;
using CommandLine;

namespace AuthyDecryptor.CLI;

class Program
{
    static void Main(string[] args)
    {
        Parser.Default.ParseArguments<CmdLineOptions>(args)
            .WithParsed(options =>
            {
                var inputFile = options.InputFile;
                var outputFile = options.OutputFile;
                var authyBackupPassword = options.Password;

                if (options.Verbose)
                {
                    ConsoleHelper.WriteVerbose($"Input file: {inputFile}");
                    ConsoleHelper.WriteVerbose($"Output file: {outputFile}");

                    if (!string.IsNullOrWhiteSpace(authyBackupPassword))
                    {
                        ConsoleHelper.WriteVerbose($"Passed secret: {new string('*', authyBackupPassword.Length)} ({authyBackupPassword.Length} characters)");
                    }
                    
                    ConsoleHelper.WriteVerbose("");
                }

                if (string.IsNullOrWhiteSpace(authyBackupPassword))
                {
                    if (options.Verbose)
                    {
                        ConsoleHelper.WriteVerbose("No password was passed. Asking user to enter it");
                    }

                    Console.WriteLine("Please enter your Authy backup password:");
                    authyBackupPassword = ConsoleHelper.GetConsoleInputHidden();
                    if (string.IsNullOrWhiteSpace(authyBackupPassword))
                    {
                        ConsoleHelper.WriteError("No password provided. Stopping ...");
                        return;
                    }
                }

                if (options.Verbose)
                {
                    ConsoleHelper.WriteVerbose("Reading input file");
                }
                var jsonData = File.ReadAllText(inputFile);

                if (options.Verbose)
                {
                    ConsoleHelper.WriteVerbose("Parsing input json");
                }
                
                var data = JsonSerializer.Deserialize<EncryptedFileData>(jsonData);
                if (data == null)
                {
                    if (options.Verbose)
                    {
                        ConsoleHelper.WriteVerbose("JsonSerializer.Deserialize returned null value.");
                    }

                    ConsoleHelper.WriteError("File was not read correctly or is empty. Stopping ...");
                    return;
                }

                if (options.Verbose)
                {
                    ConsoleHelper.WriteVerbose($"Found {data.AuthenticatorTokens.Count} tokens - start decrypting");
                }

                var decryptedTokens = new List<DecryptedToken>();
                foreach (var token in data.AuthenticatorTokens)
                {
                    if (options.Verbose)
                    {
                        ConsoleHelper.WriteVerbose("Token data to handle:");
                        ConsoleHelper.WriteVerbose($"  Name: {token.Name}");
                        ConsoleHelper.WriteVerbose($"  Unique ID: {token.UniqueId}");
                        ConsoleHelper.WriteVerbose($"  PBKDF2 iterations: {token.KeyDerivationIterations:#,0}");
                        ConsoleHelper.WriteVerbose($"  Salt: {token.Salt}");
                        ConsoleHelper.WriteVerbose($"  IV: {token.UniqueIv}");
                        ConsoleHelper.WriteVerbose($"  Encrypted seed length: {(token.EncryptedSeed?.Length.ToString() ?? "")}");
                        ConsoleHelper.WriteVerbose($"  Account type: {token.AccountType}");
                        ConsoleHelper.WriteVerbose($"  Digits count: {token.Digits}");
                        ConsoleHelper.WriteVerbose($"  Issuer: {token.Issuer}");
                        ConsoleHelper.WriteVerbose($"  Logo name: {token.Logo}");
                        ConsoleHelper.WriteVerbose("");
                    }

                    if (string.IsNullOrWhiteSpace(token.EncryptedSeed))
                    {
                        ConsoleHelper.WriteWarning($"Token '{token.Name}' has no encrypted seed. Skipping.");
                        continue;
                    }

                    if (string.IsNullOrWhiteSpace(token.Salt))
                    {
                        ConsoleHelper.WriteWarning($"Token '{token.Name}' has no salt. Skipping.");
                        continue;
                    }

                    if (string.IsNullOrWhiteSpace(token.UniqueIv))
                    {
                        ConsoleHelper.WriteWarning($"Token '{token.Name}' has no IV. Skipping.");
                        continue;
                    }

                    try
                    {
                        if (options.Verbose)
                        {
                            ConsoleHelper.WriteVerbose($"Decrypting token '{token.Name}' ...");
                        }

                        var start = DateTime.UtcNow;
                        
                        var decryptedSeed = Decryptor.DecryptToken(
                            token.KeyDerivationIterations,
                            token.EncryptedSeed,
                            token.Salt,
                            token.UniqueIv,
                            authyBackupPassword);
                        
                        var end = DateTime.UtcNow;

                        if (options.Verbose)
                        {
                            ConsoleHelper.WriteVerbose($"Token decrypted in {(end - start).TotalMilliseconds:N0} ms");
                        }
                        
                        decryptedTokens.Add(new DecryptedToken
                        {
                            AccountType = token.AccountType,
                            Name = token.Name,
                            Issuer = token.Issuer,
                            DecryptedSeed = decryptedSeed,
                            Digits = token.Digits,
                            Logo = token.Logo,
                            UniqueId = token.UniqueId
                        });
                    }
                    catch (Exception ex)
                    {
                        ConsoleHelper.WriteError($"Error decrypting token '{token.Name}'", ex);
                    }
                }

                if (options.Verbose)
                {
                    ConsoleHelper.WriteVerbose("All tokens decrypted. Writing to result file:");
                    ConsoleHelper.WriteVerbose($"  File name: {outputFile}");
                    ConsoleHelper.WriteVerbose($"  Indented: {options.OutputIndented}");
                    ConsoleHelper.WriteVerbose($"  Open file: {options.OpenOnFinish}");
                    ConsoleHelper.WriteVerbose("");
                }

                var serializerOptions = new JsonSerializerOptions(JsonSerializerOptions.Default)
                {
                    WriteIndented = options.OutputIndented == true
                };

                File.WriteAllText(outputFile, JsonSerializer.Serialize(decryptedTokens, serializerOptions), Encoding.UTF8);
                ConsoleHelper.WriteSuccess($"The tokens were decrypted successfully.{(options.OpenOnFinish == true ? " Opening file." : "")}");
                
                if (options.OpenOnFinish == true)
                {
                    try
                    {
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = outputFile,
                            Verb = "open",
                            UseShellExecute = true
                        });
                    }
                    catch (Exception ex)
                    {
                        ConsoleHelper.WriteError("Failed to open the output file.", ex);
                    }
                }
            });
    }
}
