using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GameSparks.Api.Requests;
using GameSparks.Api.Responses;
using GameSparks.Core;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemiesGenerator : MonoBehaviour
{
    private string
        names = "Ophelia;Janith;Nodab;Athalie;Brader;Ripley;Leonardi;Carole;Buhler;Knowlton;Settera;Wally;Ferren;Sylvia;Fae;Diogenes;Newbill;Abbe;Hudis;Romalda;Dasi;Henig;Weihs;Gabbey;Donny;Kathy;Danyluk;Donadee;Nissie;Sanburn;Hakon;Frederique;Beck;Tam;Millur;Kirsteni;Cates;Adelice;Anthea;Phippen;Farrish;Cox;Jansen;Ossie;Harlow;Fineman;Landes;Giffy;Androw;Binetta;Linetta;Shull;Greene;Harlan;Mullen;Pugh;Loggins;Stalder;Bohun;Haveman;Burn;Ormand;Lanta;Ricki;Milena;Corri;Nataline;Martie;Walrath;Samanthia;Hilten;Xylon;Kimitri;Lantha;Ludwig;Tomkin;Heddi;Paul;Feliza;Menis;Criswell;Joelynn;Conrad;Rayshell;Barbi;Schober;Adler;Komarek;Lathrop;Seaver;Gibb;Bibi;Karena;Aundrea;Jaella;Checani;Gradey;Lette;Glorianna;Pinto;Humo;Gorlicki;Bois;Suzann;Dirk;Gyatt;Campy;Mukund;Eziechiele;Stalker;Lovell;Arelus;Flinn;Ruperta;Hirz;Jenni;Hippel;Adin;Albina;Aubin;Shel;Thebault;Idden;Joane;Clim;Paddie;Demetris;Seward;Koosis;Loni;Moor;Cohberg;Lennox;Granny;Hike;Perice;Christis;Mecke;Fanni;Seedman;Spatola;Kahl;Hodges;Ashly;Cornall;Sprage;Isak;Bond;Trueblood;Chery;Soo;MacNamara;Caldeira;Merth;Fronia;Melisa;Galasyn;Alick;Roxie;Cohlier;Sjoberg;Joshuah;Spence;Khan;Tammy;Audsley;Chandler;Ahron;Derayne;Latrena;Rodgers;Webster;Joann;Sophronia;Dania;Pet;Stefan;Ebenezer;Gerladina;Ambler;Cahra;Australia;Fulvi;Scheers;Mahla;Proudman;Behre;Osy;Kiona;Zacherie;Skillern;Picco;Otina;MacDonald;Elish;Collyer;Sigrid;Michail;Laural;Fiertz;Woodman;Capwell;Lyndsie;Shira;Adala;Weidar;Christianson;Gilli;Lewiss;Evanthe;Autumn;Kenlee;Pascale;Moriarty;Nork;Christina;Osmen;Efthim;Rexanna;Jens;Nina;Addia;Adabel;Connie;Rainwater;Lemire;Silsbye;Hannah;Jowett;Guthrie;Kushner;Andromeda;Alison;Cicely;Edris;Stralka;Normy;Fulvia;Him;Marigolde;Jenness;Arvind;Ema;Gallard;Barbabas;Damiano;Alded;Heeley;Gorski;Giacobo;Willman;Cosmo;Newmark;Storm;Inoue;Ryun;MacRae;Sol;Eisinger;Angelia;Weeks;Nieberg;Sancho;Morentz;Conan;Crooks;Haya;Faye;Parfitt;Jago;Hauser;Maybelle;Fording;Dianthe;Jabin;Wallraff;Strade;Wallinga;Lancaster;Gehlbach;Chilson;Rhu;Hamrnand;Forsyth;Barb;Klute;Jair;Tolland;Kuehn;Thorman;Elda;Gurias;Barnett;Kalil;Poulter;Rocker;Clarey;Adele;McGill;Alviani;Matheson;Alper;Kassandra;Purse;Sathrum;Megargee;Hagan;Yevette;Tiana;Schwinn;Wong;Bay;Dopp;Cash;Wang;Aleksandr;Pren;Boy;Catie;Hermy;Henryson;McNair;Mann;Johnathan";

    public bool generate = false;
    private List<string> namesList;

    public int currentStars = 0;

    public Fighter FighterPref;
    public FighterPartInstancer FighterPartInstancer;
    public WeaponInstancer WeaponInstancer;

    public List<FighterPart> generatedParts = new List<FighterPart>();
    public int generated = 0;
    public string currentID = "";
    
    
    private void Start()
    {
        if (generate)
        {
            namesList = names.Split(';').ToList();
            Waiter.Get().WaitForSecondsC(2, () => { }, GenerateEnemies);
        }
    }

    private async void GenerateEnemies()
    {
        foreach (string name in namesList)
        {
            while (!GS.Available)
            {
                System.Threading.Thread.Sleep(100);
            }

            bool registered = await RegisterEnemy(name);
            if (!registered)
            {
                continue;
            }

            bool logged = await LoginEnemy(name);
            if (!logged)
            {
                continue;
            }

            currentStars += Random.Range(-1, 4);
            currentStars = Mathf.Max(0, currentStars);

            GenerateFighter(FighterNum.F1);
            GenerateFighter(FighterNum.F2);
            GenerateFighter(FighterNum.F3);

            new GameSparks.Api.Requests.LogEventRequest()
                .SetEventKey("Leaderboard")
                .SetEventAttribute("Scorer", currentStars)
                .Send((response) =>
                {
                    if (!response.HasErrors)
                    {
                        Debug.Log("Saved Leaderboard");
                    }
                    else
                    {
                        Debug.Log("Error Leaderboard");
                    }
                });

            GS.Reconnect();
            
            generated++;
        }

        Debug.Log("GeneratingDone");
    }

    private Task<bool> RegisterEnemy(string name)
    {
        bool registered = false;

        var t = new TaskCompletionSource<bool>();
        new GameSparks.Api.Requests.RegistrationRequest()
            .SetDisplayName(name)
            .SetPassword("test_enemy")
            .SetUserName(name)
            .Send((response) =>
                {
                    if (!response.HasErrors)
                    {
                        Debug.Log(name + " Registered");
                        registered = true;
                        Storage.PlayerID = response.UserId;
                        currentID = response.UserId;
                    }
                    else
                    {
                        Debug.Log("Error Registering " + name);
                        registered = false;
                    }

                    t.TrySetResult(registered);
                }
            );

        return t.Task;
    }

    private Task<bool> LoginEnemy(string name)
    {
        bool logged = false;

        var t = new TaskCompletionSource<bool>();
        new GameSparks.Api.Requests.AuthenticationRequest()
            .SetUserName(name)
            .SetPassword("test_enemy")
            .Send((response) =>
            {
                if (!response.HasErrors)
                {
                    Debug.Log(name + " Authenticated");
                    logged = true;
                }
                else
                {
                    Debug.Log("Error Authenticating " + name);
                    logged = false;
                }

                t.TrySetResult(logged);
            });

        return t.Task;
    }

    public void GenerateFighter(FighterNum fighterNum)
    {
        generatedParts.Clear();

        Fighter fighter = Instantiate(FighterPref);
        FighterPart butt = GeneratePart(PartType.BUTT);
        FighterPart torso = GeneratePart(PartType.TORSO);
        FighterPart head = GeneratePart(PartType.HEAD);
        FighterPart rUpperArm = GeneratePart(PartType.UPPER_ARM);
        FighterPart lUpperArm = GeneratePart(PartType.UPPER_ARM);
        FighterPart rLowerArm = GeneratePart(PartType.LOWER_ARM);
        FighterPart lLowerArm = GeneratePart(PartType.LOWER_ARM);
        FighterPart rThigh = GeneratePart(PartType.THIGH);
        FighterPart lThigh = GeneratePart(PartType.THIGH);
        FighterPart rShin = GeneratePart(PartType.SHIN);
        FighterPart lShin = GeneratePart(PartType.SHIN);

        Array Types = Enum.GetValues(typeof(WeaponType));
        Weapon rWeapon = GenerateWeapon((WeaponType) Types.GetValue(Random.Range(0, Types.Length)));
        Weapon lWeapon = GenerateWeapon((WeaponType) Types.GetValue(Random.Range(0, Types.Length)));


        fighter.FighterNum = fighterNum;

        Dictionary<BodyPartType, BodyPart> bodyParts = fighter.BodyParts.AllNamedBodyParts;

        bodyParts[BodyPartType.BUTT].FighterPart = butt;
        bodyParts[BodyPartType.TORSO].FighterPart = torso;
        bodyParts[BodyPartType.HEAD].FighterPart = head;
        bodyParts[BodyPartType.R_UPPER_ARM].FighterPart = rUpperArm;
        bodyParts[BodyPartType.L_UPPER_ARM].FighterPart = lUpperArm;
        bodyParts[BodyPartType.R_LOWER_ARM].FighterPart = rLowerArm;
        bodyParts[BodyPartType.L_LOWER_ARM].FighterPart = lLowerArm;
        bodyParts[BodyPartType.R_THIGH].FighterPart = rThigh;
        bodyParts[BodyPartType.L_THIGH].FighterPart = lThigh;
        bodyParts[BodyPartType.R_SHIN].FighterPart = rShin;
        bodyParts[BodyPartType.L_SHIN].FighterPart = lShin;

        fighter.RightArmWeapon.Weapon = rWeapon;
        fighter.LeftArmWeapon.Weapon = lWeapon;

        foreach (FighterPart generatedPart in generatedParts)
        {
            generatedPart.Upgrade(Random.Range(Mathf.RoundToInt(currentStars * .05f * .7f), Mathf.RoundToInt(currentStars * .05f * 1.3f)));
            Storage.SaveFighterPart(generatedPart);
        }

        rWeapon.Upgrade(Random.Range(Mathf.RoundToInt(currentStars * .05f* .7f), Mathf.RoundToInt(currentStars * .05f * 1.3f)));
        lWeapon.Upgrade(Random.Range(Mathf.RoundToInt(currentStars * .05f* .7f), Mathf.RoundToInt(currentStars * .05f* 1.3f)));
        Storage.SaveWeapon(rWeapon);
        Storage.SaveWeapon(lWeapon);
        Storage.SaveFighter(fighter);

        Destroy(fighter.gameObject);
        Destroy(rWeapon.gameObject);
        Destroy(lWeapon.gameObject);

        foreach (FighterPart generatedPart in generatedParts)
        {
            Destroy(generatedPart.gameObject);
        }

        generatedParts.Clear();
    }

    private FighterPart GeneratePart(PartType type)
    {
        FighterPartJson partJson = FighterPartInstancer.FighterPartsPrefs
            .Select(w => w.GetJsonClass()).First(p => p.PartType == type);
        partJson.ID = System.Guid.NewGuid().ToString();
        FighterPart part = FighterPartInstancer.GetInstance(partJson);
        switch (Random.Range(0, 3))
        {
            case 0:
                part.ItemMaterialType = ItemMaterialType.WOOD;
                break;
            case 1:
                part.ItemMaterialType = ItemMaterialType.ROCK;
                break;
            case 2:
                part.ItemMaterialType = ItemMaterialType.STEEL;
                break;
        }

        generatedParts.Add(part);
        return part;
    }

    private Weapon GenerateWeapon(WeaponType type)
    {
        WeaponJson weaponJson = WeaponInstancer.WeaponsPrefs
            .Select(w => w.GetJsonClass()).First(p => p.WeaponType == type);
        weaponJson.ID = System.Guid.NewGuid().ToString();
        Weapon weapon = WeaponInstancer.GetInstance(weaponJson);
        switch (Random.Range(0, 3))
        {
            case 0:
                weapon.ItemMaterialType = ItemMaterialType.WOOD;
                break;
            case 1:
                weapon.ItemMaterialType = ItemMaterialType.ROCK;
                break;
            case 2:
                weapon.ItemMaterialType = ItemMaterialType.STEEL;
                break;
        }

        return weapon;
    }

    Task<T> AsAsync<T>(Action<Action<T>> target)
    {
        var tcs = new TaskCompletionSource<T>();
        try
        {
            target(t => tcs.SetResult(t));
        }
        catch (Exception ex)
        {
            tcs.SetException(ex);
        }

        return tcs.Task;
    }
}