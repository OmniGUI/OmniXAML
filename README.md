# OmniXAML. The Cross-platform XAML Framework.

*I know you need it, I know you want it. I know you XAML!*

OmniXAML is a library that allows you interpret XAML with ease. You can read XAML and get the object it represents, like a Window in WPF, a document, a diagram or whatever object you can describe.

In its current state it's able to interpret more or less complex XAML without problems.

It already can deal with the some features that make XAML the coolest descriptive XML-based language, like:
- XAML namespaces
- Prefix definitions
- Content Properties
- Markup Extensions (i.e. Bindings, x:Type, StaticResource…)
- Deferred reading (DataTemplates, ControlTemplates)
- Attachable Members. (Attached Properties in WPF/WinRT)
- Type Converters. Ability to convert an instance of one type to another implicitly (usually from values in XAML that come as strings).

# Screenshots
![Test Application](https://cloud.githubusercontent.com/assets/3109851/8144539/a7bc8274-11e3-11e5-98b9-7ef890afaa5a.PNG "Test Application.")

You can read XAML and turn it into, for example, a Window like this:
![Sample XAML inflated to WPF](https://cloud.githubusercontent.com/assets/3109851/8144244/f072185e-11d3-11e5-8fc8-e3950aabc5f1.PNG "XAML inflated to WPF")

# Sample XAML
The following an example of XAML that can be read with OmniXAML.
```xml
<Window Width="300" Height="300" 
        xmlns="perspex"
        xmlns:m="clr-namespace:TestApplication;Assembly=TestApplication">
    <ScrollViewer>
        <StackPanel>
            <ListBox Items="{Binding Path=People}">
                <ListBox.DataTemplates>
                    <XamlDataTemplate DataType="{Type TypeName=m:Person}">
                        <Grid>
                            <Grid.ColumnDefinitions>
                                <ColumnDefinition Width="Auto" />
                                <ColumnDefinition />
                            </Grid.ColumnDefinitions>
                            <Image Source="github_icon.png" Height="60" />
                            <Button Grid.Column="1" Content="{Binding Path=Name}" />
                        </Grid>
                    </XamlDataTemplate>
                </ListBox.DataTemplates>
            </ListBox>
        </StackPanel>
    </ScrollViewer>
</Window>
```

This project is linked with [Perspex, the next-generation WPF](https://github.com/grokys/Perspex).

Thanks to [Nicholas Blumhardt](https://twitter.com/nblumhardt) for his awesome project [Sprache](https://github.com/sprache/Sprache) that has introduced me in the world of parsers.

```csharp
foreach (var thing in life) 
{
   world.Shout(string.Format("XAML is the best language to describe {0}", thing);
}
```
