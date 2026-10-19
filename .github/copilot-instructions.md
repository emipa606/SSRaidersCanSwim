# GitHub Copilot Instructions for [SS] Raiders Can Swim (Continued)

## Mod Overview and Purpose

[SS] Raiders Can Swim (Continued) is a modification for the game RimWorld, originally created by Spdskatr, now updated to ensure compatibility with recent game versions and expansions. The primary purpose of this mod is to introduce a new dynamic to combat strategies by allowing both raiders and colonists to swim through deep water. This adds an extra layer of strategy and planning in base defense and colony expansion.

## Key Features and Systems

- **Swimming Mechanics:** Raiders and colonists have the ability to swim across deep water, albeit at a slower pace (4.2% move speed). This alters the strategic use of water as a natural barrier.
- **Combat Changes:** While in deep water, characters are considered "lying down," which reduces their shooting accuracy, adding a risk factor to using water as a traversal method.
- **Compatibility with Other Mods:** The mod is compatible with Rainbeau's Fertile Fields, where previously impassable water tiles now become passable. Compatibility with sd Terraforming has not been tested.

## Coding Patterns and Conventions

- **C# Naming Conventions:** Class names use PascalCase, such as `RaidersCanSwim`, while method names also follow the PascalCase convention.
- **Using Harmony:** Methods are modded using Harmony patches, with Postfix and Transpiler employed to modify base game functionality without directly altering game files.
- **Code Structure:** The code structure is organized into distinct files for each specific functionality, ensuring clarity and separation of concerns.

## XML Integration

- **Patches Definition:** XML files are utilized to define patches for the game, particularly in `Patches.xml`. These files specify what game elements are affected and the conditions under which modifications occur.
- **Configuration and Extensions:** XML is used to manage configuration settings and potentially extend game functionality through simple declarative statements.

## Harmony Patching

- **Transpiler Method:** The `Transpiler` method is used extensively to adjust or replace method implementations dynamically at runtime.
- **Postfix Method:** Ensures additional logic is executed after a method completes. This is utilized for methods like `PawnRenderNodeWorker_Body_CanDrawNow`.
- **Targeted Harmony Patches:** Each patch is targeted to specific methods, allowing precise alterations of game behavior with minimal performance overhead and conflict occurrence.

## Suggestions for Copilot

- **Commenting and Documentation:** Ensure comments and documentation are included in code, especially for public-facing methods, to help Copilot provide better contextual assistance.
- **Code Examples:** Provide Copilot with usage examples, especially in methods where specific logic patterns are repeated, like swimming logic.
- **Test Coverage:** Set up automated testing scripts via Copilot to ensure all patched elements function as expected across different game scenarios and with other mods.
- **Code Refactoring:** Consider suggestions from Copilot for code refactoring to improve efficiency, especially where loops and condition checks are involved.
- **Compatibility Checks:** Use Copilot to suggest compatibility checks with other popular mods and upcoming RimWorld updates.

By following these instructions, you can better utilize GitHub Copilot to develop, maintain, and enhance the [SS] Raiders Can Swim mod, ensuring a seamless gaming experience for players. Contribute feedback and improvements directly on the GitHub repository.

## Project Solution Guidelines
- Relevant mod XML files are included as Solution Items under the solution folder named XML, these can be read and modified from within the solution.
- Use these in-solution XML files as the primary files for reference and modification.
- The `.github/copilot-instructions.md` file is included in the solution under the `.github` solution folder, so it should be read/modified from within the solution instead of using paths outside the solution. Update this file once only, as it and the parent-path solution reference point to the same file in this workspace.
- When making functional changes in this mod, ensure the documented features stay in sync with implementation; use the in-solution `.github` copy as the primary file.
- In the solution is also a project called Assembly-CSharp, containing a read-only version of the decompiled game source, for reference and debugging purposes.
- For any new documentation, update this copilot-instructions.md file rather than creating separate documentation files.


## Hard rules (must follow)
- Do NOT run commands that modify the repo (no git commit, git apply, dotnet format) unless explicitly asked.
- Prefer minimal reads: read only the smallest code region needed (around the suspicious lines).
- When mentioning SonarQube issues, automatically use the SonarQube MCP service to fetch and address issues instead of making inferred fixes without querying SonarQube first.
- When mentioning the rimworld log, automatically use the Rimworld MCP service to fetch the log.

