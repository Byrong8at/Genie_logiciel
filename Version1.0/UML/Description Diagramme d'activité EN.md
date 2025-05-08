This activity diagram allows us to represent the process of events depending on the state of the system. This diagram shows the start of the application, the processing and bacjup of works and the closing of the application once the processing is finished.   

1. Starting the application :

The application starts up.
Loads the configuration (location of journal files and states, etc.).
Verifies the availability of configured saved works.

2. User entry :

The user enters an entry in the command line specifying the works to execute (Ex : 1 - 3, 1 ; 3, or a number for a single work).
The application validates or not the user entry.

3. Work processing loop :

For each specified work :

The application recovers the configurations of the work (name, source, target, type).
The application prepares the backup and creates target directories if necessary. 
The application registers the initial state in the state file (State : "Active", Number of files, total size, etc.).
The application recursively searches the target directory. 

For each file/folder : 

A copy of the file or folder is sent to the target directory. 
The application registers the backup information in the journal file (timestamp, save name, source and target paths, file size, transfer time).
The application updates the file state with the progress (number of files processed, total size transfered, number of remaining files, space remaining, path of the file in progress of processing).  
At the end of the work, the application registers the final state in the state file (State : "Finished" or "Error").

4. End of application :

The application finishes its execution after processing all the specified works.

The objective of this diagram allows for the visualisation of the appliction functioning logic step by step and therefore shows the applications decision-making through the questions, it is simplified to be the as clear as possible so the error management that will be intergrated in each step of the process as well as the language management is not shown or mentioned. 

This diagram was created for EasySave version 1.0 specifically for delivrable 1. 
