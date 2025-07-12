using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PerkActionFactory
{
    public static IPerkAction CreatePerkAction(string id)
    {
        switch (id)
        {
            case "Per01": return new Bargain();
            case "Per02": return new Berserker();
            case "Per03": return new BlacksmithCoupon();
            case "Per04": return new Blockmaster();
            case "Per05": return new Bulwark();
            case "Per06": return new ContagiousVenom();
            case "Per07": return new CriticalHealing();
            case "Per08": return new CriticalStrength();
            case "Per09": return new Cuddly();
            case "Per10": return new DeepCuts();
            case "Per11": return new NoxiousCape();
            case "Per12": return new Enraged();
            case "Per13": return new Giantism();
            case "Per14": return new GoldenArmor();
            case "Per15": return new Greedy();
            case "Per16": return new HealingClaw();
            case "Per17": return new Hedgehog();
            case "Per18": return new Hoarder();
            case "Per19": return new MagicMirror();
            case "Per20": return new Magnetism();
            case "Per21": return new Minimalist();
            case "Per22": return new NaturalStrength();
            case "Per23": return new PoisonWeapons();
            case "Per24": return new RerollCoupon();
            case "Per25": return new Resilent();
            case "Per26": return new SavingAccount();
            case "Per27": return new Spikes();
            case "Per28": return new SuspiciousGreenRod();
            case "Per29": return new Thief();
            case "Per30": return new Weaklings();
            case "Per31": return new ShredderCoupon();
            case "Per32": return new VampireFangs();
            case "Per33": return new HardHits();
            default:
                Debug.LogWarning($"Không tìm thấy hành động cho ID: {id}");
                return null;
        }
    }
}