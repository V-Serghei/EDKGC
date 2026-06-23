# EDKGC - Educational Cryptography GUI

EDKGC is a WPF desktop application for learning cryptographic algorithms, testing RSA signatures, and running a practical ISO 27001 self-assessment with a simple annualized risk calculation.

## Current Features

- Symmetric encryption:
  - AES
  - DES
  - Triple DES
  - Blowfish
  - Twofish
  - Serpent
  - SEAL is listed but marked as not supported
- Asymmetric encryption:
  - RSA with PKCS#1 padding for short messages
  - Diffie-Hellman shared-secret flow with AES text encryption
  - ElGamal block encryption/decryption
  - ECC shared-secret flow with AES text encryption
- Digital signature:
  - SHA-256 hash generation
  - RSA/PKCS#1 signature with private key
  - Verification with public key
- ISO 27001 module:
  - localized questionnaire
  - threat-level chart and recommendations
  - risk analysis table using `ALE = SLE * EF * ARO`
- Runtime language switch:
  - English by default
  - Russian supported
  - UI strings and ISO questions are stored in JSON files

## Important Notes

- RSA directly encrypts only short messages. For long data, use hybrid encryption: encrypt a symmetric key or hash with RSA, and encrypt the large data with a symmetric algorithm.
- Diffie-Hellman and ECC are used to derive a shared secret. The application then uses AES for text encryption.
- ISO 27001 calculations are educational/self-assessment estimates, not a certification audit or legal compliance proof.
- In the risk table:
  - `SLE` is the expected monetary loss from one incident before exposure adjustment.
  - `EF` is the exposure factor from `0` to `1`.
  - `ARO` is the annualized rate of occurrence.
  - `ALE = SLE * EF * ARO`.

## Localization Data

Translations are data-driven and stored outside C# code:

```text
EDKGC/Data/Localization/ui.en.json
EDKGC/Data/Localization/ui.ru.json
EDKGC/Data/Seed/ISOQuestion.en.json
EDKGC/Data/Seed/ISOQuestion.ru.json
```

`ISOQuestion.json` is kept as a fallback for compatibility.

## Tech Stack

- C# / WPF
- .NET `net8.0-windows`
- MVVM with MvvmLight and CommunityToolkit.Mvvm
- BouncyCastle.Cryptography
- MaterialDesignThemes
- LiveCharts
- OxyPlot
- FontAwesome5
- Newtonsoft.Json

## Project Structure

```text
EDKGC/
|-- Data/
|   |-- Localization/       UI translations
|   `-- Seed/               localized ISO 27001 questions
|-- Encryption/             AES, DES, RSA, and shared crypto helpers
|-- Enams/                  enums used by algorithms and ISO answers
|-- Infrastructure/         commands, controls, converters
|-- Models/                 algorithm and ISO data models
|-- Resources/              icons, images, and shared WPF theme
|-- ViewModel/              MVVM view models
`-- Views/                  WPF windows and XAML layouts
```

## Build

Open `EDKGC/EDKGC.sln` in Rider or Visual Studio, restore NuGet packages, then build the `EDKGC` project.

The application copies JSON data from `Data/Localization/*.json` and `Data/Seed/*.json` to the output directory at build time.
