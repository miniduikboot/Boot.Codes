# Boot.Codes

## What does it do?

This Impostor plugin reads lists of 6 or 4 character long words, and will assign them to lobbies when they are created. Once the lists are exhausted, game codes will be assigned "randomly", as usual.

## 2. How to use it?

### Prerequisites

​ To install the plugin, you need [Impostor](https://github.com/Impostor/Impostor) 1.4 build 178 or newer, which can be obtained from AppVeyor. Then, you need a list of game codes. You may write your own, or you could download one of the wordlists provided in the repository. Keep in mind that codes can only be 4 or 6 characters in length.

​ Wordlists may include comments with a prefix of `--`. Lines starting with the sequence will be ignored, but lines with codes may contain comments, as long as the game code precedes the comment.

​ The wordlist(s) must be put in the server's directory, in a folder called `Boot.Codes`. The full path is printed out when starting. We're providing compatible wordlists with common English words in https://github.com/miniduikboot/Boot.Codes-lists

​ After you have a compatible Impostor server, and at least one wordlist, you may download the latest release and add the DLL to the `plugins` folder.

### Usage

​ Once you have correctly installed it, you should see a message in the console when the server is starting, showing how many codes were loaded. If you see a message indicating that no codes were loaded, your wordlists are invalid. You may also see messages warning you of invalid codes.

​ If any codes were loaded, every new lobby will take a code from the lists randomly. Though, you can only have as many custom lobbies as you have words. Once there are more lobbies than the number of codes, normal codes will start being assigned again.

## Showcase

![](readme-resources/console.png)

![](readme-resources/game0.png)

![](readme-resources/game1.png)

![](readme-resources/game2.png)

## Credits

- Empireu for taking this plugin from the prototype stage to something useful.
- Minorusama for creating the API this plugin relies on.
