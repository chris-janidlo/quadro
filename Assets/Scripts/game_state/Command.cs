using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using crass;

public class Command
{
    public readonly Com MainCom;
    public readonly ReadOnlyCollection<Com> MetaComs;

    public Com.EffectVector EffectVector =>
        MetaComs.Aggregate(MainCom.InitialVector, (vector, metaCom) => metaCom.MetaEffect(vector));

    public string Description =>
        MainCom.DescribeMainEffect(EffectVector);

    public ReadOnlyCollection<Com> AllComs => 
        new List<Com> { MainCom }.Concat(MetaComs).ToList().AsReadOnly();
    
    public Com LastCom =>
        MetaComs.Count == 0 ? MainCom : MetaComs.Last();

    public Command (Com mainCom)
    {
        MainCom = mainCom;
        MetaComs = new List<Com>().AsReadOnly();
    }

    private Command (Com mainCom, IList<Com> metaComs)
    {
        MainCom = mainCom;
        MetaComs = new ReadOnlyCollection<Com>(metaComs);
    }

    public Command PlusMetaCom (Com com)
    {
        return new Command(MainCom, MetaComs.ConcatItems(com));
    }

    public void CastOn (Player input)
    {
        MainCom.MainEffect(input, EffectVector);
    }

    public bool CanComboInto (InputDirection direction)
    {
        if (MetaComs.Count == 0)
        {
            return MainCom.GetMainComboData(direction);
        }
        else
        {
            return MainCom.GetMetaComboData(MetaComs.Last().Direction, direction);
        }
    }
}
