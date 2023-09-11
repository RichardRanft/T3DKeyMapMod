# T3DKeyMapMod
A simple tool for managing key maps in T3D 3.6 and earlier.

This tool removes the keymap block from the (approximate) middle of optionsDlg.cs and into a new file called keyMap.cs in the same folder.  It adds a line at the top of optionsDlg.cs to execute the new keyMap.cs file.  It also provides an interface for examining and changing the key map for the project selected.

It assumes that the optionsDlg.cs file is located in (project)/game/scripts/gui - select the (project) folder when opening a project.  If you need to change this, have at it.

[BasicLogging Source is available here](https://github.com/RichardRanft/BasicLogging "BasicLoggingSource is available here").

## License
T3DKeyMapMod's C# source is MIT licensed:
> Copyright (c) 2016 Richard Lee Ranft Jr.

> Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

> The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

> THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR  ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

## HTML Agility Pack
[Html Agility Pack; Microsoft Public License (Ms-PL)](https://htmlagilitypack.codeplex.com/license "Html Agility Pack; Microsoft Public License (Ms-PL)").
You can remove the Agility Pack if you wish (of course) - but as I can't remember why I was using it you might have some fixing to do.
