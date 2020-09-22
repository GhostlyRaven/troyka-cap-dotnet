# <img src="https://github.com/GhostlyRaven/troyka-cap-dotnet/raw/master/logos/amperka-32.png"></img> **troyka-cap-dotnet**

**Implementation of the library for TroykaCap from Amperka on the dotnet platform.**

**Target framework – .NET Core 3.1.**

# Installation
Install the following nuget packages:

[![NuGet version](https://img.shields.io/nuget/v/Unosquare.Raspberry.IO?color=green)](https://www.nuget.org/packages/Unosquare.Raspberry.IO/)

```
PM> Install-Package Unosquare.Raspberry.IO
```

[![NuGet version](https://img.shields.io/nuget/v/Unosquare.WiringPi?color=green)](https://www.nuget.org/packages/Unosquare.WiringPi/)

```
PM> Install-Package Unosquare.WiringPi
```

[![NuGet version](https://img.shields.io/nuget/v/TroykaCap.Expander?color=green)](https://www.nuget.org/packages/TroykaCap.Expander/)

```
PM> Install-Package TroykaCap.Expander
```

# Using the TroykaCap expander

```csharp
    public static class Program
    {
        private static ushort Pin1 = 0;
        private static ushort Pin2 = 1;

        public static void Main()
        {
            //Initializing WiringPi library
            Pi.Init<BootstrapWiringPi>();

            //Get gpio expander.
            //CreateGpioExpander - throws an exception if the object is null, the I2C address has an invalid value or the device at the specified address was not found.
            IGpioExpander expander = Pi.I2C.CreateGpioExpander();

            //SafeCreateGpioExpander - returns null if the object is null, the I2C address is invalid, or the device at the specified address was not found.
            //IGpioExpander expander = Pi.I2C.SafeCreateGpioExpander();

            Console.WriteLine("Start");

            while (Exit())
            {
                double result = expander.AnalogRead(Pin1);

                expander.AnalogWrite(Pin2, result);
            }

            Console.WriteLine("Stop");
        }

        private static bool Exit()
        {
            return !(Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape);
        }
    }
```

# Additional information

* [Compiled examples and nuget package.](https://github.com/GhostlyRaven/troyka-cap-dotnet/tree/master/binary)
* [Other examples for working with TroykaCap expander.](https://github.com/GhostlyRaven/troyka-cap-dotnet/tree/master/src/TroykaCap.Examples)
