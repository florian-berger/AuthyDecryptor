# AuthyDecryptor
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg?style=for-the-badge)](LICENSE)
[![Issues](https://img.shields.io/github/issues/florian-berger/AuthyDecryptor?logo=github&style=for-the-badge)](https://github.com/florian-berger/AuthyDecryptor)
[![Discord](https://img.shields.io/discord/302523634075828226?label=Discord&logo=discord&style=for-the-badge)](https://ultgmng.de/discord)

This tool decrypts the TOTP secrets stored by the '''Authy''' app. Why? Because in 2024, Twilio (the company behind Authy) decided to discontinue the support of their Desktop application from one day to another.

As a software developer, I must access tokens fast and easily in my everyday work. Sadly, Twilio does not provide any possibility to get access to my own tokens' secrets in plain text. And I don't want to remove and create a new token for more than 70 different accounts I protected within Authy.

Even the export of the encrypted ones requires some complex setup (as described [here by AlexTech01](https://github.com/AlexTech01/Authy-iOS-MiTM/)). If you have access to your encrypted secrets, you can use this project to decrypt them and re-create it with some alternative solutions.

## Contribution
To contribute to the development of this project, take a look at the [Contribution guidelines for this project](CONTRIBUTION.md).

## TODO
- [x] CLI tool
- [x] GUI with the ability to render secrets as QR codes
- [x] Load already decrypted files
- [x] Edit tokens and save them
- [ ] UI localization

## Syncfusion
This project uses WPF components from Syncfusion. I am not allowed and not able to license the binaries of Syncfusion as part of this project!