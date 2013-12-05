###ReSharper.DictionaryHelper

This is a plugin for [ReSharper](http://jetbrains.com/resharper) which provides a set of refactorings for `IDictionary<,>`.

###Example

```csharp
class MyClass
{
  static void MyMethod(IDictionary<int, string> dictionary, int key)
  {
    if (dictionary.ContainsKey(key)) // Warning: Optimize access to dictionary.
    {
      var z = dictionary[key];
  
      Console.WriteLine(z);
    }
  }
}
```
will be replaced to:
```csharp
class MyClass
{
  static void MyMethod(IDictionary<int, string> dictionary, int key)
  {
    string value;
    if (dictionary.TryGetValue(key, out value))
    {
      var z = value;
        
      Console.WriteLine(z);
    }
  }
}

```

###Installation

Available in [ReSharper Gallery](http://resharper-plugins.jetbrains.com/packages/ReSharper.DictionaryHelper/)

###Donations

Donations are welcome to 

* BTC: 19woiHcAZqDBLDAsi5QDVGwqxdaQawwt6J
* LTC: LP3wMjumuutC45MVwqbNitavUXFqAD8YjU
