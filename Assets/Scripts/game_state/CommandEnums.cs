using System;
using UnityEngine;
using crass;

public enum CommandButton
{
    Jab, Kick, Utility, Finisher
}

public enum CommandZone
{
    Attack, Defense, Register
}

public class CommandBox<ValueType, ButtonBoxType>
    where ButtonBoxType : CommandBox<ValueType, ButtonBoxType>.Buttons<ValueType>, new()
{
    public class Buttons<T>
    {
        public T Jab, Kick, Utility, Finisher;

        public T this [CommandButton input]
        {
            get
            {
                switch (input)
                {
                    case CommandButton.Jab: return Jab;
                    case CommandButton.Kick: return Kick;
                    case CommandButton.Utility: return Utility;
                    case CommandButton.Finisher: return Finisher;
                    default: throw new ArgumentException(message: "unexpected CommandButton value", paramName: nameof(input));
                }
            }

            set
            {
                switch (input)
                {
                    case CommandButton.Jab: Jab = value; break;
                    case CommandButton.Kick: Kick = value; break;
                    case CommandButton.Utility: Utility = value; break;
                    case CommandButton.Finisher: Finisher = value; break;
                    default: throw new ArgumentException(message: "unexpected CommandButton value", paramName: nameof(input));
                }
            }
        }
    }

    [SerializeField] ButtonBoxType _attack = new ButtonBoxType();
    public ButtonBoxType Attack => _attack;
    
    [SerializeField] ButtonBoxType _defense = new ButtonBoxType();
    public ButtonBoxType Defense => _defense;
  
    [SerializeField] ButtonBoxType _register = new ButtonBoxType();
    public ButtonBoxType Register => _register;

    public ButtonBoxType this [CommandZone input]
    {
        get
        {
            switch (input)
            {
                case CommandZone.Attack: return Attack;
                case CommandZone.Defense: return Defense;
                case CommandZone.Register: return Register;
                default: throw new ArgumentException(message: "unexpected CommandZone value", paramName: nameof(input));
            }
        }
    }
}

[Serializable]
public class CommandStrings : CommandBox<string, CommandStrings.StringButtons>
{
    [Serializable] public class StringButtons : CommandBox<string, CommandStrings.StringButtons>.Buttons<string> {}
}

[Serializable]
public class CommandBools : CommandBox<bool, CommandBools.BoolButtons>
{
    [Serializable] public class BoolButtons : CommandBox<bool, CommandBools.BoolButtons>.Buttons<bool> {}
}

[Serializable]
public class CommandMap : CommandBox<Command, CommandMap.CommandButtons>
{
    [Serializable] public class CommandButtons : CommandBox<Command, CommandMap.CommandButtons>.Buttons<Command> {}
}
