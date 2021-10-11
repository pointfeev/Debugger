# Debugger
This mod is meant to patch away bugs I've encountered while playing the game that either cause game crashes or that hurt my overall gameplay experience.

For example, I've added a few console commands under the prefix "debugger" that fix menial issues such as naked heroes (**debugger.fix_all_naked_heroes**).

For most patches, I don't actually "fix" the bugs because that's better left to the actual developers.
Rather, I simply wrap the original methods in try catches and return a fallback value should an exception occur.
