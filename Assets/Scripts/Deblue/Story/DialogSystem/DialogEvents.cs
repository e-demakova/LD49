using Deblue.Story.DialogSystem.Choices;

namespace Deblue.Story.DialogSystem
{
    public readonly struct DialogChoiceMade
    {
        public readonly Choice Choice;

        public DialogChoiceMade(Choice choice)
        {
            Choice = choice;
        }
    }

    public readonly struct DialogGaveChoice
    {
        public readonly Choice[] Choices;
        public readonly string ChoiceTextId;

        public DialogGaveChoice(Choice[] choices, string choiceTextId)
        {
            Choices = choices;
            ChoiceTextId = choiceTextId;
        }
    }

    public readonly struct DialogStarted
    {
        public readonly DialogSO Dialog;
        public readonly string CharacterId;

        public DialogStarted(DialogSO dialog, string characterId)
        {
            Dialog = dialog;
            CharacterId = characterId;
        }
    }

    public readonly struct DialogEnded
    {
        public readonly DialogSO Dialog;
        public readonly string CharacterId;

        public DialogEnded(DialogSO dialog, string characterId)
        {
            Dialog = dialog;
            CharacterId = characterId;
        }
    }

    public readonly struct ReplicaSwitched
    {
        public readonly Replica Replica;

        public ReplicaSwitched(Replica replica)
        {
            Replica = replica;
        }
    }
}