# EDKGC — Educational Cryptography GUI for .NET

A WPF desktop application for learning cryptographic algorithms and conducting ISO 27001 information security audits.

## Features

- **Symmetric encryption**: AES, DES, Triple DES
- **Asymmetric encryption**: RSA
- **Hashing**: SHA-based hash generation
- **Digital signature**: RSA document signing
- **ISO 27001 audit**: Interactive questionnaire with pie/bar chart visualization

## Tech Stack

- C# / WPF — **net8.0-windows** (SDK-style project)
- MVVM architecture (MvvmLightLibs + CommunityToolkit.Mvvm)
- NuGet: BouncyCastle.Cryptography 2.6.2, MaterialDesignThemes 5.3.2, CommunityToolkit.Mvvm 8.4.2, LiveCharts 0.9.7, OxyPlot 2.2.0, FontAwesome5, Newtonsoft.Json 13.0.4

## Project Structure

```
EDKGC/
├── Encryption/       — cryptographic algorithms (AES, DES, RSA)
├── Models/           — data models
├── ViewModel/        — MVVM ViewModels
├── Views/            — XAML views
├── Infrastructure/   — commands, converters
├── Enams/            — enumerations
├── Data/Seed/        — seed data (ISO 27001 questions)
└── Resources/        — icons and images
```

## Getting Started

1. Open `EDKGC/EDKGC.sln` in Visual Studio or Rider
2. Restore NuGet packages
3. Build and run
