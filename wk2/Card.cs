public class Card
{
    private int Suit { get; } // {1 - 4}

    private string[] SuitName = ["Spades", "Hearts", "Diamonds", "Clubs"];

    private int Value { get; } // {1 - 13}

    private string[] ValueName = ["Ace", "2", "3", "4", "5", "6", "7", "8", "9", "10", "Jack", "Queen", "King"];

    // Getters
    public int GetValue() {
        return Value;
    }

    public int GetSuit() {
        return Suit;
    }

    public string GetString() {
        return $"{ValueName[Value-1]} of {SuitName[Suit-1]}";
    }


    // Constructor
    public Card(int value, int suit) {
        if (value < 1 || value > 13 ) {
            // throw an error
        } else {
            this.Value = value;
        }
        

        if (suit < 1 || suit > 4) {
            // throw an error
        } else {
            this.Suit = suit;
        }
        
        
    }


}


///
/// Playing cards are used in many computer games, including versions of such classics as solitaire, hearts, and poker. 
/// Create a class named Card that contains both numeric and string data fields to hold a suit
///  (1 through 4 and “spades,” “hearts,” “diamonds,” or “clubs”) 
/// and both numeric and string data fields to hold a card value 
/// (1 to 13 and a string for the nonnumeric cards such as “queen”). Include get methods for each field.
/// Write an application named DisplayTwoCards that randomly selects two playing cards that are objects of the Card class.
///  Generate a random number for each card’s suit and another random number for each card’s value. 
/// Do not allow the two cards to be identical in both suit and value. 
/// Display each card’s suit and value and a message that indicates which card has the higher value or that the two cards have equal values.
/// In the card game War, a deck of playing cards is divided between two players. 
/// Each player exposes a card; the player whose card has the higher value wins possession of both exposed cards. 
/// Create a game of War named WarCardGame that stores 52 Card class objects in an array to represent each of the standard cards in a 52-card deck. Play a game of War by revealing one card for the computer and one card for the player at a time. Award two points for the player whose card has the higher value. (For this game, the king is the highest card, followed by the queen and jack, then the numbers 10 down to 2, and finally the ace.) If the computer and player expose cards with equal values in the same turn, award one point to each. At the end of the game, all 52 cards should have been played only once, and the sum of the player’s and computer’s score will be 52.