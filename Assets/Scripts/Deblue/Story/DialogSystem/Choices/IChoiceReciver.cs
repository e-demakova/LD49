using Deblue.Story.DialogSystem.Choices;

namespace Deblue.Story.DialogSystem
{
    public interface IChoiceReciver
    {
        bool CheckChoiceAvalible(Choice choice);
    }
}