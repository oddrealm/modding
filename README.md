# modding
<pre>
GETTING STARTED:

THIS PROJECT IS NOT FOR COMMERCIAL DISTRIBUTION. IT IS ONLY A TOOL TO HELP MODDERS CREATE MODS FOR ODD REALM. 
YOU HAVE PERMISSION TO COPY AND USE THE PROVIDED DATA AND TEXTURES FOR ODD REALM MODS ONLY. 
UNKNOWN ORIGIN GAMES INC. IS NOT RESPONSIBLE FOR ANY MOD THAT YOU CREATE. 

PART I: MAKING NEW GAME DATA OBJECTS:

There are many types of data objects to edit and create using this project, but, for this example, I'll walk you through creating a new item (Iron Shovel 2h).
This should give you a basic understanding of the modding pipeline.

NOTE: The data that comes with the game in Resources_moved is what Odd Realm uses by default. 
      You do not have to include any of these in your mods. 
      They are in the project for reference and use in your mods if desired.
      
1.  Download the modding Unity project.  
    (https://github.com/oddrealm/modding)
2.  Open the Unity Project (Unity Version 2019.4.34f1).
3.  Open the Addressables Profiles window.  
    Window>Asset Management>Addressables>Profiles.
4.  Create a new profile from the Profiles window (top-left).  
    Create>Profile. 
5.  Name it 'MoreTools' or whatever you wish your mod pack to be called.  
    Think of a Profile as a new mod pack.  
    This is what players will see when the mod screen appears in-game (more on this later). 
6.  With the MoreTools profile selected, change the build paths.  
    -[UnityEngine.Application.persistentDataPath]/Mods/MoreItems  
    -{UnityEngine.Application.persistentDataPath}/Mods/MoreItems  
    When the mod is ready to use in-game (all the data is set up), we 'build' (export the data) using these paths.  
    IMPORTANT: Always make sure your mods are folders withing the Mods folder of the Odd Realm save folder.   
               If a Mods folder doesn't exist, you can create one.   
               Without the mods in this folder, the game won't be able to find them.  
7.  Open the Addressables Groups window.   
    Window>Asset Management>Addressables>Groups.  
8.  Create 1 new group from the Groups window (top-left).   
    Create>Group>Packed Assets.  
    A group lets us build new data together.
    So, if you had two profiles (two mod packs), you'd want two groups, one for each.
    This way, you can assign data to each profile (mod) when building.
    This will make it so one mod isn't taking in data from another mod.
9.  Rename the group: 'MoreTools'.  
    Select the new group and change these settings (found in the inspector window):
       a. BundleMode = PackTogetherByLabel
       b. BundleNamingMode = Filename
10. Create our first 4 data objects which will be used by the game.  
    You should create:   
       'item_iron_shovel_two_hand'  
       'tooltip_item_iron_shovel_two_hand'  
       'visuals_item_iron_shovel_two_hand'  
       'accessory_item_shovel_two_hand'  
    NOTE: There are lots of other data objects (too many for the scope of this tutorial) you can create that items use, but these are four to get an item started and in-game.  
    There are two methods for creating new objects; use whichever works best for you:  
    a. In the Project window, find a similar data object, select it, and press ctrl+d to duplicate it.   
       For example, duplicate 'item_iron_tongs_one_hand', and rename it to 'item_iron_shovel_two_hand'.  
    b. In the Project window, right-click the data folder (for example, Items) in Resources_moved, and create a new scriptable object.   
       Create>Scriptable Objects>Items.  
       Rename the new object to 'item_iron_shovel_two_hand'.  
    NOTE: You can rename the new object anything.   
          Use a naming convention which works for you.   
11. Copy the object name and set it as the object 'Key' property.   
    You can see the object properties by selecting it.   
    All the properties will be displayed in the Inspector window.   
    'Key' should be at the top.  
    IMPORTANT: You have to name the key the same name as the object name.   
               This key is used as a look up id in-game.  
12. (Only applicable for items, blockmodels, blocks, and plant object types)   
    Set the object 'Index' integer property to a unique value.   
    This value is used to save and load the object in-game, and must be unique from other objects of the same type.  
    You can use the DataEdit window to help find a free index for your new object.  
    Tools>DataEdit  
    Click the items tab at the top of the window.  
    All data is ordered by index, so navigate to the bottom to find the last used index and add 1 to get your new index.  
13. Select the new item object in the Project window and set the 'TooltipID', 'Visuals', and 'AccessoryData' properties to point to the new respective data objects.  
    For example, the 'Visuals' property should now have 'visuals_item_iron_shovel_two_hand' as the key.   
    NOTE: Feel free to edit the other properties, but, for this tutorial, we'll just worry about getting the item into the game, and not about how it's fundamentally unique.  
14. Select the new tooltip object in the Project window and set the properties to:  
    'Name' = Iron Shovel 2h  
    'InlineIcon' = &lt;sprite=1262&gt;
    'InlineAndName' = &lt;sprite=1262&gt; Iron Shovel 2h  
    'Icon' = 'sp_iron_shovel_two_hand_icon'  
    These values are used to display the item in UI text fields.  
    INLINE ICONS: What is an inline icon? &lt;sprite=1262&gt;? Wha? This is a graphic that is used in text strings and the number in <sprite=9999> is the index lookup  for the texture.  
                  The index 1262 is pointing at the specific texture in our inline graphics atlas.  
                  Think Odd Realm emoji.  
                  You can see the default inline graphics texture in the Resources folder ('sp_textmesh_inline_graphics').  
                  See Part II: Making New Textures for more about inline graphics.  
15. Select the new blockvisuals object in the Project window and set the properties to:  
    'TextureX' = 70  
    'TextureY' = 128  
    These coordinates are used to tell the game what visual graphic the item should use when seen lying on the ground in-game.  
    TEXTURE ATLAS: The game has one big texture atlas used for most of the visuals.  
                   Plants, items, blocks, weather fx, etc, all use this atlas.  
                   Some things like entities, UI icons, overworld map, and inline graphics do not use this atlas.  
                   As of writing this tutorial, I still need to make the overworld map and entities textures moddable, but they are coming soon!  
                   You can see the default texture atlas in the Resources folder ('tx_terrain_atlas').  
                   See Part II: Making New Textures for more about the texture atlas.  
16. (Optional)  
    Select the new characteraccessory object in the Project window and set the properties to what you like.  
    If you duplicated the iron tongs characteraccessory, you probably won't need to edit any of the properties as the colors will already be set for iron.  
    If not, you should set the colors to what you want for your item.   
    The character accessory is the actual object that you can see on an entity.  
    The texture coordinates, like the blockvisuals coordinates are used to look up a visual in the texture atlas.  
    NOTE: Not every item needs a characteraccessory.  
          Items like Wood Logs don't have any because they are not equipped to a character.  
17. Select all 4 new objects and toggle the 'Addressables' property to TRUE.  
    This property should be in the top-left of the Inspector window.  
    When this is toggled on, the objects are added to the Addressable Groups window.  
    Window>Asset Management>Addressables>Groups.  
18. In the Addressables Groups window, drag each newly added object entry into the MoreItems group.  
    For example: drag the entry named (it should have a name like this) 'Assets/Resources_moved/Data/items/item_iron_shovel_two_hand.asset' to the 'MoreItems' group.
19. In the Addressables Groups window, set the labels for all the new entries to match exactly the data type name.  
    For the default mod project, you should already see labels for these 4 groups.  
    If not, you can create these labels, by selecting the drop-down menu for the entry (in the 'Labels' column) and then clicking 'Manage Labels'.  
    Hit the + symbol to add the label and name it to match the required group.  
    For example, 'items'.  
    IMPORTANT: Make sure the labels match the name of the data type, all lowercase, no spaces or special characters.   
               The data type is the folder the data comes from.
               For example, 'items' or 'blockvisuals'.
               Make sure the entries don't have more than one label.  
               The label is used to help find the data in-game, and if these don't have the correct naming, they won't show up.  
20. Export ('build') the data.  
    In the Addressables Groups window, select Build>New Build>Default Build Script (top-right).  
    Remember that build path from before? This will take all our new data and put it there with all the required formatting to import it into the game.  
    You'll see all this data in the Mods folder under the folder MoreTools (or whatever you named your mod).  
    IMPORTANT: Make sure not to change the names of any of these exported files.  
21. Run Odd Realm (v0.11.0.6 and higher).  
22. You should see a pop-up window appear for mods.  
    The game looks for folders in the Mods folder and will display these in this window.  
23. Enable the mod MoreTools by selecting the box.  
24. Click confirm.  

Conclusion:  
At this point you should be able to spawn and use an Iron Shovel 2h in-game.  
To quickly test out whether the item was created properly, start a new settlement, open the console window (~), and type 'spawn -i item_iron_shovel_two_hand', and hit enter.  
If everything is set up correctly, you should see your new shovel appear.  
If you don't see the visuals for the item, it's likely you haven't done Part II yet, or you've not set up the texture coords and atlas correctly.  
See PART III: Troubleshooting for some tips on how to find out what's wrong.  

PART II: MAKING NEW TEXTURES:  

We are going to create all the required textures for our new Iron Shovel 2h which was created in Part I.  
There are three types of textures to edit and mod: icon, texture atlas, and inline graphics.  
Entity atlases, and overworld map atlases will be moddable soon!  
As well, I still need to add logic to stitch multiple atlases from separate mods together.  
IMPORTANT: The game uses the first mod textures it finds when a mod is enabled.  
           This means that if you have two mods that both provide their own texture atlas or inline graphics, the first one found will be used.  
           I will try to make something to make multiple texture mods possible, but it will take a bit of work.  
           Thanks for your patience there.  

1. In the Project window, navigate to the Resources/Textures folder.  
   You should see 3 textures: 'sp_iron_shovel_two_hand_icon', 'sp_textmesh_inline_graphics', and 'tx_terrain_atlas'.  
   These textures are here for reference.   
   They aren't part of the data build pipeline and are only intended as reference for you to make new textures.  
2. Open the File Explorer (On windows. OSX and Linux will have their respective file system navigators.) by right-clicking the Resources/Textures folder and selecting 'Show In Explorer'.  
3. Select the 3 textures, right-click them, and select Copy.  
4. In the File Explorer window, navigate to the mods folder (MoreTools) you created in Part I. Right-click and paste the textures into this folder.  
   IMPORTANT: Be sure to not rename 'sp_textmesh_inline_graphics' or 'tx_terrain_atlas'.  
              These names are used to find these textures when you enable your mod in-game.  
   IMPORTANT: Make sure all textures are .png format.     
   IMPORTANT: Don't change the texture atlas sizes for the inline graphics and terrain atlas.  
              You can't change the resolution size of the game, but that's a planned mod feature.  
5. (Optional)  
   Using a program that lets you edit textures (I recommend Aseprite), open the 3 textures.    
   NOTE: The provided textures should already be set up with the Iron Shovel 2h visuals.  
         You can draw whatever you want to replace it, or leave it as is.  
6. (Optional)  
   Edit the 'tx_terrain_atlas' texture.  
   This is the main texture atlas for the game.  
   Although, it says 'terrain' it's actually used for all sorts of things, including plants, trees, items, weather fx, etc.  
   This atlas is split up into 8x8 tiles but there are not rules for where in the atlas you can put your artwork.  
   I try to group things together based on type, but do whatever you feel works for you.  
   You'll notice some scribbled integer coordinates in the atlas.  
   These coordinates are used by the blockvisuals data to display our new item (which we created in Part I) but you can erase them to make new space.  
   They are there just to help me quickly figure out the x/y coordinates.  
   I find it helpful to enable an 8x8 grid in Aseprite to show me the tile borders.  
   If you navigate to coordinates 70x 128y, you should see what looks like an iron shovel sprite.  
   NOTE: Right now you can't change the art scales, but this is something I will be adding to the mod features.  
         That way players would be able to make higher resolution art if they wished.  
7. (Optional)  
   Edit the 'sp_textmesh_inline_graphics' texture.  
   This is the main texture for Odd Realm text graphics.  
   These textures are predominantly what you see in UI windows and such.  
   Like the textur atlas, they are split into 8x8 tiles.  
   However, the coordinate system uses one integer value instead of an x/y coordinate system.  
   To figure out the index, you count from left to right, top to bottom.  
   So, the top-left tile is 0, the top-right tile is 63.  
   The next row starts on the left at 64, and ends on the right at 127.  
   Again, you'll see my scribbled numbers to make this easier, but you can erase those to free up room.  
   IMPORTANT: Any icons you draw should be 5x5 pixels.  
              They can be bigger, but 5x5 fits nicely in all the various UI layouts.  
              Bigger icons will overflow and overlap other UI elements.  
   NOTE: The green tiles are filler.  
         You can erase these and put your own graphics.  
8. (Optional)  
   Edit or create new icons.  
   In Part I, for the tooltips object, we set the field 'Icon' to 'sp_iron_shovel_two_hand_icon'.  
   The third texture we brought over into our mod folder, 'sp_iron_shovel_two_hand_icon', is what this refers to.  
   You can duplicate and create as many textures as you wish in this folder, and, as long as they don't have the same name as the inline graphics and terrain atlas, will be loaded by the game for use in the HUD.  
   NOTE: 16x16 is the standard size for icons in the UI.  
         You can use different sizes, but it will not have the correct pixel ratio and might look strange.  
        

Conclusion:  
Just like Part I, you should be able to see these in game by booting Odd Realm, and then enabling the mod.    
You'll likely have to re-boot the game any time you change the textures to see the new changes.  
    
PART III: TROUBLESHOOTING      

You will likely encounter lots of annoying issues as you try to create mods, especially because modding Odd Realm is so new.  
There are thousands of object properties to change and play around with, and I guarantee you'll change something to a value that I haven't anticipated.  
When something doesn't work, the first place to look for clues as to why it's not working, is the log files.  
Player.log and Player-Prev.log (the log for the previous game session) are two text files that show all the log information when the game is running.  
It's not guaranteed this will show an error or important warning, but sometimes it can help.  
For example, you should see logs for when the game finds texture mods.  
  
Going forward, I'll be adding stuff to help communicate issues, but I'll need your suggestions and feedback first.  
Please email me with questions or feedback at waylon@oddrealmgame.com.  
Also, we have a fantastic discord community with a dedicated modding channel.  
Here's the link: https://discord.gg/6bw3k6w   
Please come join us and feel free to ping me there with questions or feedback.  

PART IV: BUILDING MULTIPLE MODS SEPARATELY
Let's say you have followed the steps in Part I for two mod packs (profiles & groups) and you want to build them out, but keep them separate so in-game you can choose between the two:
1. With the AddressablesGroups window open, select the profile for the new mod you want to build.
   This will determine which folder to create in the Mods folder.
2. Select which groups are built into the mod (profile).
   Do this by selecting the groups you don't want to build, and (in the inspector window) disable the toggle named IncludeInBuild under the Advanced Options section.
   Then, for the groups you want to include, enable the toggle name IncludeInBuild.
3. Follow the same build steps in Part I.
   If you build two profiles correctly, you should see two separate mod folders in the Mods folder.
   And, in-game, there will be two options to enable/disable in the mod pop-up window.

</pre>
