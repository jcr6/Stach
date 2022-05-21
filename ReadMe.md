# Stach

Work in Progress.... Not usable yet!
Many features may not work properly yet... if in fact they work at all!

# Goal of the project

GJCR is a nice tool, but the current version didn't really suit me anymore and I did feel some improvements were required, and that is where Stach will come in.
The name "Stach" is a referrence to the main protagonist of the Dutch children's novel "Koning van Katoren", written by Jan Terlouw.


# What can Stach do?

Stach is just a viewing tool for JCR6 files. It can show the contents of a JCR6 files as well the data inside them, as some extra data stored in the file table about these files.

# Does Stach only support JCR6?

Heck no, it has also support for the next formats:

- WAD (Where's all the data) by id Software
  - WAD is used in DOOM, DOOM II, Rise of the Triad, Heretic, HeXeN beyond Heretic and a few more games. Stach is completely able to read WAD files, however what should be taken in order is that JCR6 is used to read WAD, and JCR6 will always put all file names in alphabetic order. Also as map files contain the same filenames in each map, JCR6 will create a folder where all these subfiles can be found in. That foldername will get the map header file's name plus the prefix "MAP". So "E1M1" will be found in folder "MAP_E1M1" and you will see the real E1M1 as just an empty file. Please note, the current version of Stach has no support for the data formats used in DOOM. So it can only show you the WAD files contents, and... that's all.
- PAK (Pack) by id Software
  - This format is used in Quake I. I really do NOT know if Stach supports Quake II, as I never had any files from that game, I could not work that out. 
- PAK (Westwood)
  - This format has been used in the Kyrandia games and Lands of Lore: The Throne of Chaos. Now I must note that this is a really messy file format and by all means far beyond any level of amateurism, and due to that I hope all PAK files from Westwoord work properly for there's no guarantee. One real downside is that unlike JCR and id's PAK and WAD formats, PAK has no way to be properly recognized, and JCR6 will merely try to see if it's finds any files in accordance to the PAK rules Westwood set up, and if it fails it will deem it not a PAK and if it succeeds it will be deemed a PAK. Due to this false positives and false negatives when it comes to recognizing a PAK file can happen, although I expect them to be rare.
- JCR5 (Jeroen's Collected Resource version 5)
  - Added for backward compatibility with my older projects. However JCR5 has been discontinued for ages by now, and all my projects using JCR5 are bound to be reviewed to see if they should be remade or not. When these reviews and remakes(if they are in order) are actually done, it's likely JCR5 support in Stach and in JCR6 in general could be considered "deprecated".
