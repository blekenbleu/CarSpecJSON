# CarSpecJSON
 Convert CarSpec JSON to C#  
	*new to me*: convert nullable `Dictionary<string, List<CarSpec>>? json` to not:  `json!`
- *5 Oct 2024*  dll size for trivial dictionary:  8 kB
    - [iterate dictionary](https://code-maze.com/csharp-iterate-through-dictionary/)
    - [iterate list](https://www.tutorialsteacher.com/articles/foreach-loop-in-csharp)  
	- test each nullable `CarSpec` member; ignore if null or empty string or zero value.
