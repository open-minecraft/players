# Open Minecraft Players

This repository contains a list of 6.7M real UUIDs of Minecraft players.

The list is represented by binary file [players.bin](/players.bin) where every 16 bytes represent a player UUID.

Original file is taken from [this](https://hypixel.net/threads/minecraft-uuid-list-6-700-000.5029428/) Hypixel thread.

## How to read the data

Pseudo code for reading the file data:
```python
file = open("players.bin", mode: 'binary')
uuids = array[ file.bytes / 16 ]

for i in range(0, uuids.count):
  array[i] = uuid.fromBytes(file.readBytes(16))
```

## Tools

You can use `ompt` tool from [Releases](https://github.com/open-minecraft/players/releases) if you want...
- ...to [check](#checking-uuid) if some uuid is in the list
- ...to [add](#adding-uuid) a new uuid
- ...to [humanize](#humanize-file-content) file content

### Checking UUID

In order to check if players list contains some UUID, run:
```
ompt check <uuid> -f <file>
```

As a result you will see `found` or `not found`.

### Adding UUID

In order to add a new UUID to players list, run:
```
ompt add <uuid> -f <file>
```

### Humanizing

Text representation of UUID takes 2x more space comparing to raw bytes. Hovewer, it is possible to convert binary data to text file.

In order to huminize players list, run:
```
ompt humanize <file> -o <output>
```