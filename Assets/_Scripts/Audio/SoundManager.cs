using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Pool;
using Random = UnityEngine.Random;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// This script is responsible for spawning objects that play sounds. <para/>
/// Btw this is a script I copy and use for all my games so that's why some features are pretty unnecesarry.
/// </summary> - Ruben
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Sounds")]
    [SerializeField] internal Sound[] sounds;
    [SerializeField] internal SoundGroup[] soundGroups;

    //private static ObjectPool<SoundObject> pool = new ObjectPool<SoundObject>(PoolCreateFunc);

    [Space]
    [SerializeField] private AudioMixerGroup soundEffectMixer;

#if UNITY_EDITOR
    // Auto Naming
    [HideInInspector] public bool HaveAutoNaming = false;
    [HideInInspector] public bool FancyAutoNaming = true;

    // Auto tools
    [HideInInspector] public List<AudioClip> AudioClipQueue = new List<AudioClip>();
#endif

    /*
    private static SoundObject PoolCreateFunc()
    {
        GameObject soundGameObject = new GameObject("Sound (" + pool.CountAll + ")", typeof(AudioSource), typeof(SoundObject));
        
        soundGameObject.GetComponent<AudioSource>().playOnAwake = false;

        SoundObject soundObj = soundGameObject.GetComponent<SoundObject>();
        soundObj.Created();
        soundObj.Pool = pool;

        DontDestroyOnLoad(soundGameObject);
        soundGameObject.transform.SetParent(Instance.transform);

        return soundObj;
    }
    */

    /// <summary>
    /// Awake is called when the script instance is being loaded
    /// </summary>
    private void Awake()
    {
        // Check if there already exists an instance of this object
        if (Instance == null)
        {
            // If not, then set this object to be the instance
            Instance = this;

            // Also set this object to dont destroy on load
            transform.SetParent(null); // We set parent to null so unity won't scream at me with a warning
            DontDestroyOnLoad(gameObject);

            /*
            // Create a new GameObject and use it as the sound template when we create the pool
            GameObject soundGameObject = new GameObject("Sound", typeof(AudioSource), typeof(SoundObject));

            SoundObject sound = soundGameObject.GetComponent<SoundObject>();

            // Create sound pool
            pool = new ObjectPool<SoundObject>(sound, maxSounds,
            (obj) =>
            {
                DontDestroyOnLoad(obj.gameObject);
            });

            // Destroy the original sound game object seing as we no longer need it
            Destroy(soundGameObject);
            */
        }
        else
        {
            // Destroy this object because a sound manager already exists
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Creates a gameobject that will play the given clip with the options inserted. The new object will then self destruct when the sound is finished playing.
    /// </summary>
    /// <param name="clip">The audio clip to play.</param>
    /// <param name="volume">The volume of the sound.</param>
    /// <param name="pitch">The pitch of the sound.</param>
    /// <param name="outputGroup">The output audio mixer group that the sound will be played to.</param>
    /// <param name="position">Where the sound will be played.</param>
    /// <param name="is3D">The spacial blend of the sound.<para/>
    /// True = is only heard in a certain area. The radius of the area is controlled by the <paramref name="range"/> parameter.<para/>
    /// False = is heard anywhere regardless of <paramref name="position"/>.</param>
    /// <param name="range">How far the sound can be heard from.<para/>
    /// X = min distance. Y = max distance.<para/>
    /// Only works if <paramref name="is3D"/> is true.</param>
    /// <param name="playInBothEars">If the sound should be able to play in both ears.<para/>
    /// Only works if <paramref name="is3D"/> is true.</param>
    public static void PlaySoundClip(AudioClip clip, float volume, float pitch, AudioMixerGroup outputGroup, Vector3 position, bool is3D, Vector2 range, bool playInBothEars = false)
    {
        // Get sound from the pool
        GameObject obj = new GameObject("Sound " + clip.name);

        AudioSource audio = obj.AddComponent<AudioSource>();

        audio.dopplerLevel = 0;
        audio.clip = clip;
        audio.volume = volume;
        audio.pitch = pitch;
        audio.outputAudioMixerGroup = outputGroup;
        audio.spatialBlend = is3D ? 1 : 0;
        audio.spread = playInBothEars ? 0 : 180;
        audio.minDistance = range.x;
        audio.maxDistance = range.y;

        // Play the sound
        audio.Play();

        KillObjectAfterTime killScript = obj.AddComponent<KillObjectAfterTime>();
        killScript.Lifetime = clip.length * pitch + 0.1f;
        killScript.UnscaledTime = true;
    }

    /// <summary>
    /// Will play a sound with customizable options.
    /// </summary>
    /// <param name="name">The name of the sound. Be warned that this is case sensitive.</param>
    /// <param name="position">Where the sound will be played.</param>
    /// <param name="pitch">The pitch of the sound.</param>
    /// <param name="is3D">The spacial blend of the sound.<para/>
    /// True = is only heard in a certain area. The radius of the area is controlled by the <paramref name="range"/> parameter.<para/>
    /// False = is heard anywhere regardless of <paramref name="position"/>.</param>
    /// <param name="range">How far the sound can be heard from.<para/>
    /// X = min distance. Y = max distance.<para/>
    /// Only works if <paramref name="is3D"/> is true.</param>
    /// <param name="playInBothEars">If the sound should be able to play in both ears.<para/>
    /// Only works if <paramref name="is3D"/> is true.</param>
    /// <returns>If the sound was played successfully or not.</returns>
    public static bool PlaySound(string name, Vector3 position, float pitch, float volume, bool is3D, Vector2 range, bool playInBothEars)
    {
        // If the name == null or empty then just don't play anything
        if (string.IsNullOrEmpty(name))
        {
            return false;
        }

        // Get the sound manager and store it in a variable
        SoundManager soundManager = Instance;

        // If the SoundManager doesn't exist then return
        if (soundManager == null)
        {
            return false;
        }

        // Create a new temporary volume variable.
        float tempVolume = 0;

        // Find right audio clip
        AudioClip clip = null;

        foreach (Sound s in soundManager.sounds)
        {
            // Check if the names are the same. If it is then the right sound has been found!
            if (s.name == name)
            {
                // Get the clip
                clip = s.clip;
                tempVolume = s.soundVolume;
                pitch += s.pitchSlider - 1;
                break;
            }
        }

        // If the clip is still null, then check for a grouped sound instead
        if (clip == null)
        {
            foreach (SoundGroup s in soundManager.soundGroups)
            {
                // Check if the names are the same. If it is then the right sound has been found!
                if (s.groupName == name)
                {
                    // Get a random clip from the sound group
                    clip = s.clips[Random.Range(0, s.clips.Length)];
                    tempVolume = s.soundVolume;
                    pitch += s.pitchSlider - 1;
                    break;
                }
            }

            // If the clip is still null, then it means we couldn't find any matching audio clip
            if (clip == null)
            {
                return false;
            }
        }

        volume *= tempVolume;

        // Play the sound
        PlaySoundClip(clip, volume, pitch, soundManager.soundEffectMixer, position, is3D, range, playInBothEars);

        // The sound was successfully played!
        return true;
    }

    /// <summary>
    /// Plays a single 2D sound with randomized pitch. The random pitch is between 0.8 and 1.2.
    /// </summary>
    /// <param name="name">The name of the sound. Be warned that this is case sensitive.</param>
    /// <returns>If the sound was played successfully or not.</returns>
    public static bool PlaySound(string name)
    {
        return PlaySound(name, Vector3.zero, Random.Range(0.8f, 1.2f), 1, false, Vector2.zero, false);
    }

    /// <summary>
    /// Plays a single 2D sound with randomized pitch. The random pitch is between 0.8 and 1.2.
    /// </summary>
    /// <param name="name">The name of the sound. Be warned that this is case sensitive.</param>
    /// <returns>If the sound was played successfully or not.</returns>
    public static bool PlaySound(string name, float pitch, float volume)
    {
        return PlaySound(name, Vector3.zero, pitch, volume, false, Vector2.zero, false);
    }

    /// <summary>
    /// Plays a single 2D sound with customizable pitch.
    /// </summary>
    /// <param name="name">The name of the sound. Be warned that this is case sensitive.</param>
    /// <param name="pitch">The pitch of the sound.</param>
    /// <returns>If the sound was played successfully or not.</returns>
    public static bool PlaySound(string name, float pitch)
    {
        return PlaySound(name, Vector3.zero, pitch, 1, false, Vector2.zero, false);
    }

    /// <summary>
    /// Plays a single 3D sound with randomized pitch. The random pitch is between 0.8 and 1.2.
    /// </summary>
    /// <param name="name">The name of the sound. Be warned that this is case sensitive.</param>
    /// <param name="position">Where the sound will be played.</param>
    /// <param name="range">How far the sound can be heard from.<para/>
    /// X = min distance. Y = max distance.</param>
    /// <param name="playInBothEars">If the sound should be able to play in both ears.</param>
    /// <returns>If the sound was played successfully or not.</returns>
    public static bool PlaySound(string name, Vector3 position, Vector2 range, bool playInBothEars = false)
    {
        return PlaySound(name, position, Random.Range(0.8f, 1.2f), 1, true, range, playInBothEars);
    }

    /// <summary>
    /// Plays a single 3D sound with customizable pitch.
    /// </summary>
    /// <param name="name">The name of the sound. Be warned that this is case sensitive.</param>
    /// <param name="position">Where the sound will be played.</param>
    /// <param name="pitch">The pitch of the sound.</param>
    /// <param name="range">How far the sound can be heard from.<para/>
    /// X = min distance. Y = max distance.</param>
    /// <param name="playInBothEars">If the sound should be able to play in both ears.</param>
    /// <returns>If the sound was played successfully or not.</returns>
    public static bool PlaySound(string name, Vector3 position, float pitch, Vector2 range, bool playInBothEars = false)
    {
        return PlaySound(name, position, pitch, 1, true, range, playInBothEars);
    }

    /// <summary>
    /// Is simply just the public version of play sound made for UI elements to reference
    /// </summary>
    public void PlayUISound(string name)
    {
        PlaySound(name);
    }
}

/// <summary>
/// Class that contains data for one single sound.
/// </summary>
[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    [Range(0, 10)]
    public float soundVolume = 1;
    [Range(0, 2)]
    public float pitchSlider = 1;
}

/// <summary>
/// Class that contains data for a group of sounds. The clip that is played is random.
/// </summary>
[System.Serializable]
public class SoundGroup
{
    public string groupName;
    public AudioClip[] clips;
    [Range(0, 10)]
    public float soundVolume = 1;
    [Range(0, 2)]
    public float pitchSlider = 1;
}

#if UNITY_EDITOR
/// <summary>
/// The custom editor for the Sound Manager.
/// </summary>
[CustomEditor(typeof(SoundManager))]
public class SoundManagerEditor : Editor
{
    private int oldSoundLength;
    private List<string> oldSoundClipNames = new List<string>();

    private int oldSoundGroupLength;
    private List<List<string>> oldSoundGroupsClipNames = new List<List<string>>();

    // This object
    private SoundManager soundManager;

    private SerializedProperty audioClipQueue;

    public override void OnInspectorGUI()
    {
        // Begin GUI Check
        EditorGUI.BeginChangeCheck();

        base.OnInspectorGUI();

        // If the GUI has changed, then trigger OnChange()
        if (EditorGUI.EndChangeCheck())
        {
            OnChange();
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Inspector Preferences", EditorStyles.boldLabel);

        // Auto naming toggles
        soundManager.HaveAutoNaming = EditorGUILayout.Toggle(new GUIContent("Have Auto Naming", "When enabled, sound names will automatically change to exactly match their sound clip."), soundManager.HaveAutoNaming);
        if (soundManager.HaveAutoNaming)
        {
            soundManager.FancyAutoNaming = EditorGUILayout.Toggle(new GUIContent("Fancy Auto Naming", "When enabled, auto names will now name the sounds like how Unity automatically names your serialized variables."), soundManager.FancyAutoNaming);
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Auto Tools", EditorStyles.boldLabel);

        // Auto tools buttons
        if (GUILayout.Button("Auto name all Sounds"))
        {
            Undo.RecordObject(soundManager, "Auto name all Sounds");

            // First change single sounds
            List<Sound> newSoundList = soundManager.sounds.ToList();

            foreach (Sound sound in soundManager.sounds)
            {
                if (sound.clip != null)
                {
                    sound.name = soundManager.FancyAutoNaming ? FancyAutoNaming(sound.clip.name) : sound.clip.name;
                }
            }

            soundManager.sounds = newSoundList.ToArray();

            // Second change group sounds
            List<SoundGroup> newSoundGroupList = soundManager.soundGroups.ToList();

            foreach (SoundGroup soundGroup in soundManager.soundGroups)
            {
                for (int i = 0; i < soundGroup.clips.Length; i++)
                {
                    if (soundGroup.clips[i] != null)
                    {
                        soundGroup.groupName = soundManager.FancyAutoNaming ? FancyAutoNaming(soundGroup.clips[i].name) : soundGroup.clips[i].name;
                    }
                }
            }

            soundManager.soundGroups = newSoundGroupList.ToArray();
        }

        if (GUILayout.Button("Remove all duplicate names"))
        {
            Undo.RecordObject(soundManager, "Remove all duplicates");

            // First single sounds
            List<Sound> newSoundList = soundManager.sounds.ToList();
            List<string> registeredNames = new List<string>();

            foreach (Sound sound in soundManager.sounds)
            {
                if (registeredNames.Contains(sound.name))
                {
                    newSoundList.Remove(sound);
                }
                else
                {
                    registeredNames.Add(sound.name);
                }
            }

            soundManager.sounds = newSoundList.ToArray();

            // Second group sounds
            List<SoundGroup> newSoundGroupList = soundManager.soundGroups.ToList();
            List<string> registeredGroupNames = new List<string>();

            foreach (SoundGroup soundGroup in soundManager.soundGroups)
            {
                if (registeredGroupNames.Contains(soundGroup.groupName))
                {
                    newSoundGroupList.Remove(soundGroup);
                }
                else
                {
                    registeredGroupNames.Add(soundGroup.groupName);
                }
            }

            soundManager.soundGroups = newSoundGroupList.ToArray();
        }

        if (GUILayout.Button("Remove all null clips"))
        {
            Undo.RecordObject(soundManager, "Remove all null clips");

            // First single sounds
            List<Sound> newSoundList = soundManager.sounds.ToList();

            foreach (Sound sound in soundManager.sounds)
            {
                if (sound.clip == null)
                {
                    newSoundList.Remove(sound);
                }
            }

            soundManager.sounds = newSoundList.ToArray();

            // Second group sounds
            foreach (SoundGroup soundGroup in soundManager.soundGroups)
            {
                List<AudioClip> newClips = soundGroup.clips.ToList();

                int amountRemoved = 0;
                for (int i = 0; i < soundGroup.clips.Length; i++)
                {
                    if (soundGroup.clips[i] == null)
                    {
                        newClips.RemoveAt(i - amountRemoved);
                        amountRemoved++;
                    }
                }

                soundGroup.clips = newClips.ToArray();
            }
        }

        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(audioClipQueue);

        // Only draw audio clip queue buttons if the field is open
        if (audioClipQueue.isExpanded)
        {
            if (GUILayout.Button("Add all as seperate single sounds"))
            {
                Undo.RecordObject(soundManager, "Added audio clip queue as seperate single sounds");

                List<Sound> newSoundList = soundManager.sounds.ToList();

                foreach (AudioClip clip in soundManager.AudioClipQueue)
                {
                    if (clip == null)
                        continue;

                    Sound newSound = new Sound()
                    {
                        clip = clip,
                        name = FancyAutoNaming(clip.name),
                        pitchSlider = 1,
                        soundVolume = 1,
                    };

                    newSoundList.Add(newSound);
                }

                soundManager.sounds = newSoundList.ToArray();

                soundManager.AudioClipQueue.Clear();
            }

            if (GUILayout.Button("Add all as one single sound group"))
            {
                Undo.RecordObject(soundManager, "Added audio clip queue as one single sound group");

                List<SoundGroup> newSoundGroupList = soundManager.soundGroups.ToList();

                List<AudioClip> newClips = new List<AudioClip>();

                foreach (AudioClip clip in soundManager.AudioClipQueue)
                {
                    if (clip == null)
                        continue;

                    newClips.Add(clip);
                }

                SoundGroup newSoundGroup = new SoundGroup()
                {
                    clips = newClips.ToArray(),
                    groupName = FancyAutoNaming(newClips[0].name),
                    pitchSlider = 1,
                    soundVolume = 1,
                };

                newSoundGroupList.Add(newSoundGroup);

                soundManager.soundGroups = newSoundGroupList.ToArray();

                soundManager.AudioClipQueue.Clear();
            }

            if (GUILayout.Button("Clear audio clip queue"))
            {
                Undo.RecordObject(soundManager, "Clear audio clip queue");

                soundManager.AudioClipQueue.Clear();
            }
        }

        // Apply modified properties
        serializedObject.ApplyModifiedProperties();
    }

    /// <summary>
    /// Called when any non "Inspector Preferences" value is changed
    /// </summary>
    private void OnChange()
    {
        // Check if the length has changed
        if (oldSoundLength != soundManager.sounds.Length)
        {
            // Check if sounds have been added (the new length is longer than the old)
            if (soundManager.sounds.Length > oldSoundLength)
            {
                Sound newSound = soundManager.sounds[soundManager.sounds.Length - 1];

                if (string.IsNullOrEmpty(newSound.name) && newSound.clip == null && newSound.soundVolume == 0 && newSound.pitchSlider == 0)
                {
                    newSound.name = "New Sound";
                    newSound.soundVolume = 1;
                    newSound.pitchSlider = 1;
                }
            }

            // Set new length
            oldSoundLength = soundManager.sounds.Length;
        }

        // Check if the length has changed
        if (oldSoundGroupLength != soundManager.soundGroups.Length)
        {
            // Check if sounds have been added (the new length is longer than the old)
            if (soundManager.soundGroups.Length > oldSoundGroupLength)
            {
                SoundGroup newSoundGroup = soundManager.soundGroups[soundManager.soundGroups.Length - 1];

                if (string.IsNullOrEmpty(newSoundGroup.groupName) && newSoundGroup.clips.Length <= 0 && newSoundGroup.soundVolume == 0 && newSoundGroup.pitchSlider == 0)
                {
                    newSoundGroup.groupName = "New Sound Group";
                    newSoundGroup.soundVolume = 1;
                    newSoundGroup.pitchSlider = 1;
                }
            }

            // Set new length
            oldSoundGroupLength = soundManager.soundGroups.Length;
        }

        // Check if auto naming is enabled
        if (soundManager.HaveAutoNaming)
        {
            if (soundManager.sounds.Length >= 1)
            {
                // Loop through all sounds and check for the specific sound that has had a clip name change
                for (int i = 0; i < soundManager.sounds.Length; i++)
                {
                    Sound newSound = soundManager.sounds[i];

                    if (newSound.clip == null)
                        continue;

                    string name = newSound.clip.name;

                    bool changeName = false;

                    if (i > oldSoundClipNames.Count - 1)
                    {
                        changeName = true;
                    }
                    else if (name != oldSoundClipNames[i])
                    {
                        changeName = true;
                    }

                    if (changeName)
                    {
                        newSound.name = soundManager.FancyAutoNaming ? FancyAutoNaming(name) : name;
                    }
                }

                // Update names
                oldSoundClipNames.Clear();
                foreach (Sound sound in soundManager.sounds)
                {
                    if (sound.clip != null)
                    {
                        oldSoundClipNames.Add(sound.clip.name);
                    }
                    else
                    {
                        oldSoundClipNames.Add(null);
                    }
                }
            }

            if (soundManager.soundGroups.Length >= 1)
            {
                // Loop through all sound groups and check for the specific sound that has had a clip name change
                for (int i = 0; i < soundManager.soundGroups.Length; i++)
                {
                    SoundGroup newSound = soundManager.soundGroups[i];

                    if (i > oldSoundGroupsClipNames.Count - 1 || newSound.clips.Length <= 0)
                    {
                        break;
                    }
                    else
                    {
                        for (int c = 0; c < newSound.clips.Length; c++)
                        {
                            if (newSound.clips[c] == null)
                            {
                                continue;
                            }

                            AudioClip clip = newSound.clips[c];

                            string name = newSound.clips[c].name;

                            bool changeName = false;

                            if (c > oldSoundGroupsClipNames[i].Count - 1)
                            {
                                break;
                            }
                            else if (clip.name != oldSoundGroupsClipNames[i][c])
                            {
                                changeName = true;
                            }

                            if (changeName)
                            {
                                newSound.groupName = soundManager.FancyAutoNaming ? FancyAutoNaming(name) : name;
                            }
                        }
                    }
                }

                // Update names
                oldSoundGroupsClipNames.Clear();
                for (int i = 0; i < soundManager.soundGroups.Length; i++)
                {
                    oldSoundGroupsClipNames.Add(new List<string>());

                    for (int c = 0; c < soundManager.soundGroups[i].clips.Length; c++)
                    {
                        AudioClip clip = soundManager.soundGroups[i].clips[c];

                        if (clip == null)
                        {
                            oldSoundGroupsClipNames[i].Add(null);
                        }
                        else
                        {
                            oldSoundGroupsClipNames[i].Add(clip.name);
                        }
                    }
                }
            }
        }

    }

    private string FancyAutoNaming(string name)
    {
        string newName = name;

        newName.Trim();

        newName.Replace('_', ' ');
        newName.Replace('-', ' ');
        newName.Replace('.', ' ');
        newName.Replace(':', ' ');
        newName.Replace(',', ' ');

        newName = AddSpacesToSentence(newName);

        newName = FirstLettersUpper(newName);

        // Remove numbers at the end
        int stepsGoneBack = 1;

        while (char.IsNumber(newName[newName.Length - stepsGoneBack]) && newName.Length - stepsGoneBack >= 0)
        {
            stepsGoneBack++;
        }

        string oldName = newName;
        newName = "";
        for (int i = 0; i < oldName.Length - stepsGoneBack + 1; i++)
        {
            newName += oldName[i];
        }

        return newName.Trim();
    }

    private string AddSpacesToSentence(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return "";

        string newText = text[0].ToString();

        for (int i = 1; i < text.Length; i++)
        {
            if (char.IsUpper(text[i]) && text[i - 1] != ' ')
                newText += ' ';

            newText += text[i];
        }
        return newText;
    }

    // Called when the object gets enabled
    private void OnEnable()
    {
        // Get the SoundManager component of this object
        soundManager = (SoundManager)target;

        audioClipQueue = serializedObject.FindProperty("AudioClipQueue");
    }

    /// <summary>
    /// Will return the given string except only the first letters are uppercase. Example: hEllO woRlD = Hello World.
    /// </summary>
    /// <param name="text">The text that will be converted</param>
    /// <param name="onlyFirstWord">If true, makes only the first words first letter uppercase and everything else lowercase</param>
    private static string FirstLettersUpper(string text, bool onlyFirstWord = false)
    {
        if (onlyFirstWord)
        {
            //Take the first letter and make it uppercase
            //Take the other letters and make them lowercase
            return char.ToUpper(text[0]) + text.Substring(1).ToLower();
        }
        else
        {
            string returnData = "";
            bool nextUpper = true;

            foreach (char c in text)
            {
                if (nextUpper)
                {
                    returnData += char.ToUpper(c);
                    nextUpper = false;
                }
                else
                {
                    returnData += char.ToLower(c);
                }

                //If the character was a space or period then the next character will be uppercase
                if (c.Equals(' ') || c.Equals('.'))
                {
                    nextUpper = true;
                }
            }

            return returnData;
        }
    }
}
#endif
