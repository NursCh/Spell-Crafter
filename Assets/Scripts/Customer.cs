

using Unity.VisualScripting;
using UnityEngine;

public class Customer : MonoBehaviour
{
    public Spell spell { get; set; }
    public int Difficulty { get; set; }
    
    public string text { get; set; }

    public void CreateCustomer(Book book)
    {
        spell = book.Spells[UnityEngine.Random.Range(0, book.Spells.Count)];
        this.Difficulty = UnityEngine.Random.Range(0, 3);
        switch (spell.name)
        {
            case "heal":
                text = "Hey, can ya <b>heal me</b>, bud?";
                break;
            case "kill":
                text = "Can you <b>take out</b> that old rat?";
                break;
            case "create":
                text = "Can you <b>double</b> this gold pile?";
                break;
            case "remove":
                text = "I have this diary with my secrets, can you make it <b>vanish?</b>";
                break;
        }
        
    }
}

