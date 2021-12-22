using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace RFG
{
  [CustomEditor(typeof(Character))]
  public class CharacterEditorWindow : Editor
  {
    public enum AddAbilityType { Select, AttackAbility }
    public enum AddMovementStateType { Select, Jump, Fall, Run, PrimaryAttack, SecondaryAttack }
    private VisualElement rootElement;
    private Editor editor;
    private AddAbilityType addAbilityType;
    private AddMovementStateType addMovementStateType;

    public void OnEnable()
    {
      rootElement = new VisualElement();

      rootElement.LoadRootStyles();
    }

    public override VisualElement CreateInspectorGUI()
    {
      rootElement.Clear();

      UnityEngine.Object.DestroyImmediate(editor);
      editor = Editor.CreateEditor(this);
      IMGUIContainer container = new IMGUIContainer(() =>
      {
        if (target)
        {
          OnInspectorGUI();

          var boldtext = new GUIStyle(GUI.skin.label);
          boldtext.fontStyle = FontStyle.Bold;

          EditorGUILayout.Space();

          Character character = (Character)target;

          EditorGUILayout.LabelField("Movement States", boldtext);
          AddMovementStateType newAddMovementStateType = (AddMovementStateType)EditorGUILayout.EnumPopup("Add Movement State: ", addMovementStateType);

          if (!newAddMovementStateType.Equals(addMovementStateType))
          {
            addMovementStateType = newAddMovementStateType;
            AddNewMovementState();
            addMovementStateType = AddMovementStateType.Select;
          }

          if (character.CharacterType == CharacterType.Player)
          {
            EditorGUILayout.LabelField("Abilities", boldtext);
            AddAbilityType newAddAbilityType = (AddAbilityType)EditorGUILayout.EnumPopup("Add Ability: ", addAbilityType);

            if (!newAddAbilityType.Equals(addAbilityType))
            {
              addAbilityType = newAddAbilityType;
              AddNewAbility();
              addAbilityType = AddAbilityType.Select;
            }
          }
          else if (character.CharacterType == CharacterType.AI)
          {
            if (GUILayout.Button("Add Brain"))
            {
              character.gameObject.GetOrAddComponent<AIBrainBehaviour>();
              character.gameObject.GetOrAddComponent<RFG.BehaviourTree.BehaviourTreeRunner>();
            }
            if (GUILayout.Button("Add Aggro"))
            {
              character.gameObject.GetOrAddComponent<Aggro>();
            }
          }


        }
      });
      rootElement.Add(container);

      return rootElement;
    }

    private void AddNewMovementState()
    {
      Character character = (Character)target;
      switch (addMovementStateType)
      {
        case AddMovementStateType.Jump:
          character.MovementState.StatePack.GenerateJumpState();
          break;
        case AddMovementStateType.Fall:
          character.MovementState.StatePack.GenerateFallState();
          break;
        case AddMovementStateType.Run:
          character.MovementState.StatePack.GenerateRunState();
          break;
        case AddMovementStateType.PrimaryAttack:
          character.MovementState.StatePack.GeneratePrimaryAttackState();
          break;
        case AddMovementStateType.SecondaryAttack:
          character.MovementState.StatePack.GenerateSecondaryAttackState();
          break;
      }
    }

    private void AddNewAbility()
    {
      Character character = (Character)target;
      switch (addAbilityType)
      {
        case AddAbilityType.AttackAbility:
          character.MovementState.StatePack.GenerateAttackAbilityStates();
          break;
      }
    }
  }
}