# Expand World Rivers

Allow configuring rivers, strems, water level and waves.

Always back up your world before making any changes!

Install on all clients and on the server (modding [guide](https://youtu.be/L9ljm2eKLrk)).

# Configuration

Settings are automatically reloaded when changing the configuration (either with [Configuration manager](https://valheim.thunderstore.io/package/Azumatt/Official_BepInEx_ConfigurationManager/) or by saving the config file). This can lead to weird behavior so it's recommended to make a fresh world after you are done configuring.

Note: Pay extra attention when loading old worlds. Certain configurations can modify the terrain significantly and destroy your buildings.

Note: Generating the minimap takes about 15 seconds. This can be disabled in the config and manually done with the command `ew_map`.

Note: Old configuration from Expand World is automatically migrated to this mod.

## Water

- Water level (default `30` meters): Sets the altitude of the water. This is not fully implemented and probably causes some glitches.
- Wave multiplier (default `1`): Multiplies the wave size. Base wave size depends on the water depth.
- Wave only height (default `true`): Multiplier only affects the height. If disabled, multiplier also affects other directions. Not fully sure which one is better.

## Lakes

Lakes are needed to generate rivers. The code searches for points with enough water and then merges them to lake objects. Use command `ew_lakes` to show their positions on the map.

Note: Lake object is an abstract concept, not a real thing. So the settings only affect river generation.

Settings to find lakes:

- Lake search interval (default: `128` meters): How often a point is checked for lakes (meters). Increase to find more smaller lakes.
- Lake depth (default: `-20` meters): How deep the point must be to be considered a lake. Increase to find more shallow lakes.
- Lake merge radius (default: `800` meters): How big area is merged to a single lake. Decrease to get more lakes.

## Rivers

Rivers are generated between lakes. So generally increasing the amount of lakes also increases the amount of rivers.

However the lakes must have terrain higher than `Lake point depth` between them. So increase that value removes some of the rivers.

Settings to place rivers:

- River seed: Seed which determines the order of lakes (when selected by random). By default derived from the world seed.
- Lake max distance 1 (default: `2000` meters): Lakes within this distance get a river between them. Increase to place more and longer rivers.
- Lake max distance 2 (default: `5000` meters): Fallback. Lakes without a river do a longer search and place one river to a random lake. Increase to enable very long rivers without increasing the total amount that much. 
- River max altitude (default: `50` meters): The river is not valid if this terrain altitude is found between the lakes.
- River check interval (default: `128` meters): How often the river altitude is checked. Both `River max altitude` and `Lake point depth`.

Rivers have params:

- River seed: Seed which determines the random river widths. By default derived from the world seed.
- River maximum width (default: `100`): For each river, the maximum width is randomly selected between this and `River minimum width`.
- River minimum width (default: `60`): For each river, the minimum width is randomly selected between this and selected maximum width. So the average width is closer to the `River minimum width` than the `River maximum width`.
- River curve width (default: `15`): How wide the curves are.
- River curve wave length (default: `20`): How often the river changes direction.

## Streams

Streams are generated by trying to find random points within an altitude range. 

- Stream seed: Seed which determines the stream positions. By default derived from the world seed.
- Max streams (default: `3000`): How many times the code tries to place a stream. This is NOT scaled with the world radius.
- Stream search iterations (default: `100`): How many times the code tries to find a suitable start and end point.
- Stream start min altitude (default: `-4` meters): Minimum terrain height for stream starts.
- Stream start max altitude (default: `1` meter): Maximum terrain height for stream starts.
- Stream end min altitude (default: `6` meters): Minimum terrain height for stream ends.
- Stream end max altitude (default: `14` meters): Maximum terrain height for stream ends.

Streams have params:

- Stream seed: Seed which determines the random stream widths. By default derived from the world seed.
- Stream maximum width (default: `20`): For each stream, the maximum width is randomly selected between this and `Stream minimum width`.
- Stream minimum width (default: `20`): For each stream, the minimum width is randomly selected between this and selected maximum width. So the average width is closer to the `Stream minimum width` than the `Stream maximum width`.
- Stream min length (default: `80` meters): Minimum length for streams.
- Stream max length (default: `299` meters): Maximum length for streams.
- Stream curve width (default: `15`): How wide the curves are.
- Stream curve wave length (default: `20`): How often the stream changes direction.
