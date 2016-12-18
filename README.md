
It's here. This is OmniXAML V2!

# OmniXAML 
## The Cross-platform XAML Framework.

[![Join the chat at https://gitter.im/SuperJMN/OmniXAML](https://badges.gitter.im/Join%20Chat.svg)](https://gitter.im/SuperJMN/OmniXAML?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)
[![Build status](https://ci.appveyor.com/api/projects/status/yyryrdbik5snckqh?svg=true)](https://ci.appveyor.com/project/SuperJMN/omnixaml)

*I know you need it, I know you want it. I know you XAML!*

![Sample](https://cloud.githubusercontent.com/assets/3109851/8272107/1af21840-1837-11e5-85d5-e61c7c8e9679.png "Test Application")


**OmniXAML is a library that allows you interpret XAML with ease. You can read XAML and get the object it represents, like a Window in WPF, a document, a diagram or whatever object you can describe.**

In its current state it's able to interpret quite complex XAML without problems.

It complies with most of the features that XAML provides, except for some uncommon/advanced features like:
- x:Class directive
- x:TypeArguments
- x:Shared

It also lacks support for events. 

OmniXAML doesn't generate compiled XAML, so no intermediate format is produced. Since it's designed to be cross-platform, it doesn't rely on extra build steps. This means that right, in order to have access to named elements (x:Name/Name) you will have to use namescopes, for instance `window.Find(nameOfControl)`.

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

This project is linked with [Avalonia, the Next Generation Cross-Platform GUI technology](https://github.com/AvaloniaUI/Avalonia).

Thanks to [Nicholas Blumhardt](https://twitter.com/nblumhardt) for his awesome project [Sprache](https://github.com/sprache/Sprache) that has introduced me in the world of parsers.

```csharp
foreach (var thing in life) 
{
   world.Shout(string.Format("XAML is the best language to describe {0}", thing);
}
```

