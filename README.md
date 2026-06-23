# EDKGC — Educational Cryptography GUI for .NET

WPF-приложение на C# (.NET Framework) для изучения алгоритмов шифрования и аудита информационной безопасности по стандарту ISO 27001.

## Возможности

- **Симметричное шифрование**: AES, DES, Triple DES
- **Асимметричное шифрование**: RSA
- **Хэширование**: генерация хэш-значений
- **Электронная подпись**: RSA-подпись документов
- **ISO 27001**: интерактивный аудит с визуализацией результатов (pie/bar charts)

## Технологии

- C# / .NET Framework
- WPF (MVVM-архитектура)
- BouncyCastle.Cryptography
- System.Text.Json

## Структура проекта

```
EDKGC/
├── Encryption/       — алгоритмы шифрования (AES, DES, RSA)
├── Models/           — модели данных
├── ViewModel/        — MVVM ViewModels
├── Views/            — XAML-представления
├── Infrastructure/   — команды, конвертеры
├── Enams/            — перечисления
├── Data/Seed/        — начальные данные (ISO 27001 вопросы)
└── Resources/        — иконки и изображения
```

## Начало работы

1. Открыть `EDKGC/EDKGC.sln` в Visual Studio или Rider
2. Восстановить NuGet пакеты (`Restore NuGet Packages`)
3. Собрать и запустить проект
