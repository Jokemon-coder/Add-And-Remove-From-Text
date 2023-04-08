# A console application used to add, edit and delete data from a text file

## Introduction
I started this project off mainly as something to play around with classes, but it eventually turned into a rudimentary CRUD-project. The user can input information into the console, which is then added to the "People.txt" file. This data, which is stored as lines within the file with their own unique identifiers, can also be modified and deleted completely. 

Everything the program shows to the user is in Finnish, but the code itself and the comments in it are in English

## Functionality and features
The program contains a Class "Human", which contains the variables for name, age and gender. Every time the creation of a new item starts, a new object is created using the class "Human". The user then inputs the name, age and gender of the person. The variables declared for the object are then added into the file using StreamWriter and a custom ToSring-function. The unique ID for the added "person" is also created automatically based on the line where the new "person" is added. 

The name, age and gender of items contained within the text file can also be modified after creation. The ID however cannot be modified directly and is only changed if an item is deleted and its position is moved within the file as a result. The editing of the data is done by functions, which take the entire item and split it apart using specific instructions on where and how to split. Then the corresponding split section is replaced with whatever the user writes to replace it and that modified is used to overwrite and replace the original entry in the text file. 

Most of the functions relating to manipulation of the text file and it's contents are stored within the Human Class itself in public form, so they can be called from Program. 

## Known bugs or missing features as of writing



