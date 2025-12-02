using UnityEngine;
using UnityEditor;
using PixelCrushers.DialogueSystem;
using System.IO;
using System.Collections.Generic;

namespace DialogueImporter
{
    /// <summary>
    /// Custom importer for Twine JSON files exported from our conversion script.
    /// Imports the dialogue into the Unity Dialogue System database.
    /// </summary>
    public class TwineJsonImporter : EditorWindow
    {
        private DialogueDatabase database;
        private Template template;
        private string jsonFolderPath = "";
        private bool createNewActors = true;
        private Dictionary<string, int> characterActorMap = new Dictionary<string, int>();
        
        [MenuItem("Tools/Import Twine JSON")]
        public static void ShowWindow()
        {
            GetWindow<TwineJsonImporter>("Twine JSON Import");
        }

        private void OnEnable()
        {
            template = TemplateTools.LoadFromEditorPrefs();
        }

        private void OnGUI()
        {
            GUILayout.Label("Twine JSON Importer", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            database = EditorGUILayout.ObjectField("Dialogue Database", database, typeof(DialogueDatabase), false) as DialogueDatabase;
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("JSON Folder", jsonFolderPath);
            if (GUILayout.Button("Browse", GUILayout.Width(80)))
            {
                jsonFolderPath = EditorUtility.OpenFolderPanel("Select Twine JSON Folder", "", "");
            }
            EditorGUILayout.EndHorizontal();

            createNewActors = EditorGUILayout.Toggle("Create New Actors", createNewActors);

            EditorGUILayout.Space();

            EditorGUI.BeginDisabledGroup(database == null || string.IsNullOrEmpty(jsonFolderPath));
            if (GUILayout.Button("Import All Twine Stories", GUILayout.Height(30)))
            {
                ImportAllStories();
            }
            EditorGUI.EndDisabledGroup();
        }

        private void ImportAllStories()
        {
            if (database == null)
            {
                EditorUtility.DisplayDialog("Error", "Please assign a Dialogue Database.", "OK");
                return;
            }

            if (string.IsNullOrEmpty(jsonFolderPath) || !Directory.Exists(jsonFolderPath))
            {
                EditorUtility.DisplayDialog("Error", "Please select a valid folder containing JSON files.", "OK");
                return;
            }

            var jsonFiles = Directory.GetFiles(jsonFolderPath, "*.json");
            if (jsonFiles.Length == 0)
            {
                EditorUtility.DisplayDialog("Error", "No JSON files found in the selected folder.", "OK");
                return;
            }

            int importCount = 0;
            characterActorMap.Clear();

            // Pre-scan for unique character names
            foreach (var jsonFile in jsonFiles)
            {
                try
                {
                    string jsonContent = File.ReadAllText(jsonFile);
                    TwineStory story = JsonUtility.FromJson<TwineStory>(jsonContent);
                    
                    foreach (var passage in story.passages)
                    {
                        string characterName = ExtractCharacterName(passage.text);
                        if (!string.IsNullOrEmpty(characterName) && !characterActorMap.ContainsKey(characterName))
                        {
                            // Find or create actor
                            var actor = database.GetActor(characterName);
                            if (actor == null && createNewActors)
                            {
                                actor = CreateActor(characterName);
                            }
                            if (actor != null)
                            {
                                characterActorMap[characterName] = actor.id;
                            }
                        }
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogWarning($"Error pre-scanning {Path.GetFileName(jsonFile)}: {e.Message}");
                }
            }

            // Import each story
            foreach (var jsonFile in jsonFiles)
            {
                try
                {
                    ImportStory(jsonFile);
                    importCount++;
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"Failed to import {Path.GetFileName(jsonFile)}: {e.Message}\n{e.StackTrace}");
                }
            }

            EditorUtility.SetDirty(database);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            EditorUtility.DisplayDialog("Import Complete", 
                $"Successfully imported {importCount} of {jsonFiles.Length} Twine stories.", 
                "OK");
        }

        private void ImportStory(string jsonFile)
        {
            Debug.Log($"Importing: {Path.GetFileName(jsonFile)}");

            string jsonContent = File.ReadAllText(jsonFile);
            TwineStory story = JsonUtility.FromJson<TwineStory>(jsonContent);

            if (story == null || story.passages == null || story.passages.Length == 0)
            {
                Debug.LogWarning($"Story is empty or invalid: {Path.GetFileName(jsonFile)}");
                return;
            }

            // Create or find conversation
            var conversation = database.GetConversation(story.name);
            if (conversation == null)
            {
                int newConvId = database.conversations.Count > 0 
                    ? database.conversations[database.conversations.Count - 1].id + 1 
                    : 1;
                conversation = template.CreateConversation(newConvId, story.name);
                database.conversations.Add(conversation);
            }
            else
            {
                // Clear existing entries
                conversation.dialogueEntries.Clear();
            }

            // Set conversation title
            Field.SetValue(conversation.fields, "Title", story.name);
            
            // Set default actors (first found will be Actor, second will be Conversant)
            var actorSet = false;
            var conversantSet = false;

            // Create START entry
            var startEntry = template.CreateDialogueEntry(0, conversation.id, "START");
            startEntry.isRoot = true;
            Field.SetValue(startEntry.fields, "Sequence", "None()");
            conversation.dialogueEntries.Add(startEntry);

            // Import passages as dialogue entries
            foreach (var passage in story.passages)
            {
                int entryId = int.Parse(passage.pid);
                string characterName, dialogueText;
                ExtractDialogue(passage.text, out characterName, out dialogueText);

                var entry = template.CreateDialogueEntry(entryId, conversation.id, passage.name);
                entry.isRoot = false;

                // Set dialogue text
                Field.SetValue(entry.fields, "Dialogue Text", dialogueText);

                // Set actor based on character name
                if (!string.IsNullOrEmpty(characterName) && characterActorMap.ContainsKey(characterName))
                {
                    int actorId = characterActorMap[characterName];
                    Field.SetValue(entry.fields, "Actor", actorId.ToString());
                    entry.ActorID = actorId;

                    if (!actorSet)
                    {
                        conversation.ActorID = actorId;
                        actorSet = true;
                    }
                    else if (!conversantSet && actorId != conversation.ActorID)
                    {
                        conversation.ConversantID = actorId;
                        conversantSet = true;
                    }
                }

                // Set position from Twine data
                if (!string.IsNullOrEmpty(passage.position))
                {
                    var pos = passage.position.Split(',');
                    if (pos.Length == 2)
                    {
                        float x = float.Parse(pos[0]);
                        float y = float.Parse(pos[1]);
                        entry.canvasRect = new Rect(x, y, DialogueEntry.CanvasRectWidth, DialogueEntry.CanvasRectHeight);
                    }
                }

                conversation.dialogueEntries.Add(entry);
            }

            // Link START to first passage (startnode)
            int startnodeId = int.Parse(story.startnode);
            startEntry.outgoingLinks.Add(new Link(conversation.id, 0, conversation.id, startnodeId));

            // Create links between passages based on [[Link]] syntax
            foreach (var passage in story.passages)
            {
                int originId = int.Parse(passage.pid);
                var originEntry = conversation.GetDialogueEntry(originId);

                // Extract links from text
                var links = ExtractLinks(passage.text);
                foreach (var linkName in links)
                {
                    // Find destination passage by name
                    var destPassage = System.Array.Find(story.passages, p => p.name == linkName);
                    if (destPassage != null)
                    {
                        int destId = int.Parse(destPassage.pid);
                        originEntry.outgoingLinks.Add(new Link(conversation.id, originId, conversation.id, destId));
                    }
                }
            }

            Debug.Log($"Imported conversation: {story.name} with {story.passages.Length} passages");
        }

        private Actor CreateActor(string actorName)
        {
            int newActorId = database.actors.Count > 0 
                ? database.actors[database.actors.Count - 1].id + 1 
                : 3; // Start after Player and NPC

            var actor = template.CreateActor(newActorId, actorName, false);
            
            database.actors.Add(actor);
            Debug.Log($"Created new actor: {actorName} (ID: {newActorId})");
            
            return actor;
        }

        private string ExtractCharacterName(string text)
        {
            // Extract character name from "CharacterName: dialogue" format
            if (string.IsNullOrEmpty(text)) return "";
            
            int colonIndex = text.IndexOf(':');
            if (colonIndex > 0 && colonIndex < 50) // Reasonable name length
            {
                string potentialName = text.Substring(0, colonIndex).Trim();
                // Make sure it's not a link syntax
                if (!potentialName.Contains("[") && !potentialName.Contains("]") && !potentialName.Contains("("))
                {
                    return potentialName;
                }
            }
            return "";
        }

        private void ExtractDialogue(string text, out string characterName, out string dialogueText)
        {
            characterName = ExtractCharacterName(text);
            
            if (!string.IsNullOrEmpty(characterName))
            {
                int colonIndex = text.IndexOf(':');
                dialogueText = text.Substring(colonIndex + 1).Trim();
            }
            else
            {
                dialogueText = text;
            }

            // Remove links from dialogue text for cleaner display
            dialogueText = System.Text.RegularExpressions.Regex.Replace(dialogueText, @"\[\[.*?\]\]", "").Trim();
        }

        private List<string> ExtractLinks(string text)
        {
            var links = new List<string>();
            var matches = System.Text.RegularExpressions.Regex.Matches(text, @"\[\[(.*?)\]\]");
            
            foreach (System.Text.RegularExpressions.Match match in matches)
            {
                string linkText = match.Groups[1].Value;
                
                // Handle "Display Text|Link Target" format
                if (linkText.Contains("|"))
                {
                    var parts = linkText.Split('|');
                    links.Add(parts[1].Trim());
                }
                else
                {
                    links.Add(linkText.Trim());
                }
            }
            
            return links;
        }
    }

    // Data structures matching our JSON export format
    [System.Serializable]
    public class TwineStory
    {
        public string name;
        public string startnode;
        public string creator;
        public string ifid;
        public TwinePassage[] passages;
    }

    [System.Serializable]
    public class TwinePassage
    {
        public string pid;
        public string name;
        public string tags;
        public string position;
        public string size;
        public string text;
    }
}
