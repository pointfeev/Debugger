# Debugger
This mod is meant to patch away bugs I've encountered while playing the game that either cause game crashes or that hurt my overall gameplay experience, both from the vanilla game itself and sometimes from other mods that I use.

So far as TaleWorlds doesn't change anything SubModule-related, this mod should work for every version of the game; my methods of patching simply ignore a method if it doesn't exist and the fallbacks only occur if something is actually wrong.

It also adds a few console commands under the prefix "debugger" that attempt to fix menial, mostly save-related issues (such as debugger.fix_all_naked_heroes).

For most patches, I don't actually "fix" the bugs because that's better left to TaleWorlds and respective mod developers. Rather, I focus on preventing and/or fixing the effects of the bugs so I can continue playing the game.

The mod depends entirely on Harmony; I currently either utilize a prefix to prevent the method from running if something is wrong, or I simply finalize the method if the latter is not possible. I try to refrain from using finalizers as much as possible however, as their use of a try-catch can be expensive.
