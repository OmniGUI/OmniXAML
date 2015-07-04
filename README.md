# OmniXAML 
## The Cross-platform XAML Framework.

[![Join the chat at https://gitter.im/SuperJMN/OmniXAML](https://badges.gitter.im/Join%20Chat.svg)](https://gitter.im/SuperJMN/OmniXAML?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

*I know you need it, I know you want it. I know you XAML!*

![Sample](https://cloud.githubusercontent.com/assets/3109851/8272107/1af21840-1837-11e5-85d5-e61c7c8e9679.png "Test Application")

**OmniXAML is a library that allows you interpret XAML with ease. You can read XAML and get the object it represents, like a Window in WPF, a document, a diagram or whatever object you can describe.**

In its current state it's able to interpret more or less complex XAML without problems.

It already can deal with the some features that make XAML the coolest descriptive XML-based language, like:
- XAML namespaces
- Prefix definitions
- Content Properties
- Markup Extensions (i.e. Bindings, x:Type, StaticResource…)
- Deferred reading (DataTemplates, ControlTemplates)
- Attachable Members. (Attached Properties in WPF/WinRT)
- Type Converters. Ability to convert an instance of one type to another implicitly (usually from values in XAML that come as strings).

# Using OmniXAML with WPF 
It's super easy! Just follow this [simple guide](https://github.com/SuperJMN/OmniXAML/wiki/Using-OmniXAML-for-WPF).

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
