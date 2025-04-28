using CommandLine;

namespace AuthyDecryptor.CLI;

public class CmdLineOptions
{
    [Option('v', "verbose", Required = false, HelpText = "Allows the application to run in verbose mode for detailed log outputs.")]
    public bool Verbose { get; set; }

    [Option('i', "input", Required = true, HelpText = "Input file containing the encrypted data.")]
    public string InputFile { get; set; } = string.Empty;

    [Option('o', "output", Required = true, HelpText = "Output file the decrypted data should be stored in.")]
    public string OutputFile { get; set; } = string.Empty;

    [Option('p', "password", Required = false, HelpText = "Authy backup password that will be used for decryption.")]
    public string? Password { get; set; }

    [Option("indented", Default = true, HelpText = "Setting if the output json should be indented.")]
    public bool? OutputIndented { get; set; }

    [Option("open", Default = false, HelpText = "Allows opening the file when the decryption was finished.")]
    public bool? OpenOnFinish { get; set; }
}
