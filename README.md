## WinFormsHelper
Why? Because our school computers struggle to run Visual Studio!

## Info
The Helper class adds the option to the form to move, resize or change some attributes of the controls.\
You can switch between _move_ and _resize_ mode with **F2** and open the properties form with **F3**.

## Usage
The [Helper.cs](Husvet/Helper.cs) file contains all the necessary code. The rest is just an old random school assignment than I test with.\
To add the helper functions to a form just call [`Helper.AddHelper(Form form)`](Husvet/Program.cs#L16)\
If you have `Click` or `Mouse` events, there is a chance that things get messy.
