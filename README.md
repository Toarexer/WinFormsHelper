## WinFormsHelper
Why? Because our school computers struggle to run Visual Studio! (and just because)

## Usage
The Helper class adds the option to the form to move, resize or change some attributes of the controls.\
You can toggle the _edit_ mode with **F1**, switch between _move_ and _resize_ mode with **F2** and open the _properties form_ with **F3**.\
This can help to test out control locations, sizes and how changes affect them during runtime without the need to recompile the program.\
\
To add the helper functions to a form just call `Helper.AddHelper(form)`.\
If you have `Click` or `Mouse` events, there is a chance that things get messy.
