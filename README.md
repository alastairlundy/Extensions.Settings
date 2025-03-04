# Extensions.Settings
Abstractions and some base implementations around reading to and writing from Settings Storage.

## Using this Project - Intro

### Definitions
**Keys**: A key is a string that can be used to call upon a value it is associated with.
**Values**: A value is a variable that is associated with a specific key.

**Settings**: Together, Keys and their corresponding values make up Settings.

**Store**: A store is a means of storing Settings in a specified manner or format. This can for example be a text file, a JSON file, or a database table.

**Store Provider**: A Store Provider reads from and writes to a store directly. 

Within this library, you can use Store Providers to work directly with Stores.

### Assumptions
This library makes some assumptions about Settings Keys and Values:
* Keys are assumed to always be ``string``
* Values are allowed flexibility and are of the generic type ``TValue``  - This does require providing Store Providers with a Type Converter that can convert a string value to a TValue object and vice versa.

Implemented Store Providers can be used freely with minimal configuration. Using strings as the type of Values should greatly simplify this.


Within this library, a Store Provider implementing class is a class that enables reading and writing settings from a Settings Store that the Store Provider is compatible with.

Using an implemented Store Provider is preferred where possible, though the existing Store Provider implementations may not be suitable for every use case.
Where a suitable one doesn't exist, you can create your own StoreProvider, but that is a more advanced use case.

### Implemented Store Providers
These are ready to use implementations of the base ``IStoreProvider<TValue>`` Store Provider interface:
* ``TextFileStoreProvider`` - Included in the base package. A Store Provider that works with text files that contain a keys and values separated by a keySeparator char; the separator char is commonly an equals char ``=``. You can specify a different one if need be.
* ``CachedTextFileStoreProvider`` - Included in the base package. It is functionally identical to the ``TextFileStoreProvider`` except that it loads Settings into a Dictionary that it uses as a Cache to prevent constantly reading to disk. The cache is designed to expire 1 hour after creation by default but this is configurable.


### Extensibility
The following interfaces exist but are not implemented by default:
* ``IDatabaseSettingsStore``

These are the standard interfaces within the base package:
* ``ISettingsStore``
* ``ICachedSettingsStore`` - An interface to enable creating Cached Store Providers.
* ``IFileSettingsStore`` - An interface to enable creating File Store Providers.

## How to build the code
To build the library in Debug mode run the code by entering the dotnet command ``dotnet build -c Debug`` or right-click on the project file in your IDE and select "Build Selected Project" with the Debug configuration selected.

To build the library in Release mode run the code by entering the dotnet command ``dotnet build -c Release /p:ContinuousIntegrationBuild=true`` or right-click on the project file in your IDE and select "Build Selected Project" with the release configuration selected.

## How to contribute to the project
If you'd like to contribute code to the project please open an issue discussing the changes you'd like to make.

If I approve of the changes, please open a pull request and we'll see about getting the changes merged.

## Roadmap
This project doesn't aim to provide every possible implementation of Store Providers. 

Store Provider implementations for commonly used Store types are welcome, and future versions will create sub-packages that focus on those store providers such as XML, JSON, and INI files.
A few Database store provider implementation classes may also be added in the future under their own sub-packages.

Future versions may also add more interfaces or models that assist with working with Stores.

Beyond that, enhancements and improvements, extension methods for Store Providers, and other features are also possibilities for the future.

## Credits/ Acknowledgements

### Projects
This project would like to thank the following projects for their work:
* [Polyfill](https://github.com/SimonCropp/Polyfill) for simplifying .NET Standard 2.0 & 2.1 support

For more information, please see the THIRD_PARTY_NOTICES file.