# Saves

## SAVEDGAMES.ARS
This file consists of individual save slots glued together (one followed by another),  nothing special.

## Individual save slot
If the game isn't modified, then every save slot should have a size of 0x328 and follow this structure:

_(Every string is null terminated, and the maximum size written here doesn't take the terminator into account.)_

| **Offset** | **Format** | **Description**                                                                                                               |
|------------|------------|-------------------------------------------------------------------------------------------------------------------------------|
| 0x00       | int        | Constant `0x12345678`                                                                                                         |
| 0x04       | string     | Player name. Maximum size: 0xD                                                                                                |
| 0x12       | string     | Selected car .txt data file. Maximum size: 0x20                                                                               |
| 0x32       | string     | Slot save date. Maximum size: 0xA                                                                                             |
| 0x52       | string     | Slot save hour. Maximum size: 0xE                                                                                             |
| 0x74       | int[]      | Number of times a race was completed. Array size depends on the races in RACES.TXT, but the next offsets are still hard-coded |
| 0x204      | int        | Credits                                                                                                                       |
| 0x208      | int        | Difficulty. 0 = Easy, 1 = Normal, 2 = Hard                                                                                    |
| 0x20C      | int        | Game completed indicator. 0 = No, 1 = Yes. If yes, all cars are unlocked and the arrows to select any race are enabled.       |
| 0x210      | int        | Available cars to select                                                                                                      |
| 0x214      | int[]      | Available car index in slot. Array size is hard-coded to 0x3C                                                                 |
| 0x304      | int        | Current car index. For example: 0x00 = `Eagle 3`, 0x27 = `DIGGA`                                                              |
| 0x308      | int        | Current race index. For example: 0x00 = `NICE BEAVER`, 0x0F = `MISSION: TRUCKING HELL`                                        |
| 0x30C      | int        | Mission enabled indicator. 0 = No, 1 = Yes                                                                                    |
| 0x310      | int        | Armor slots in use                                                                                                            |
| 0x314      | int        | Power slots in use                                                                                                            |
| 0x318      | int        | Offensive slots in use                                                                                                        |
| 0x31C      | int        | Available Armor slots                                                                                                         |
| 0x320      | int        | Available Power slots                                                                                                         |
| 0x324      | int        | Available Offensive slots                                                                                                     |