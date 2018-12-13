using System.Collections.Generic;
using CI.QuickSave;
using UnityEngine;

namespace Utils
{
    public class SaveTest : MonoBehaviour
    {
        public Transform go;
        public Vector3 pos;
        public Quaternion rot;

        public BodyParts BodyParts;

        public bool saveB;
        public bool loadB;

        private void Start()
        {
            /*
            QuickSaveSettings settings = new QuickSaveSettings();
            settings.SecurityMode = SecurityMode.Base64;
            QuickSaveWriter writer = QuickSaveWriter.Create("TestRoot2", settings);
            writer.Write("pos", go.position);
            writer.Write("rot", go.rotation);
            writer.Commit();
            
            QuickSaveReader reader = QuickSaveReader.Create("TestRoot2", settings);
            pos = reader.Read<Vector3>("pos");
            rot = reader.Read<Quaternion>("rot");
            */
            /*

            PosRot pr = new PosRot {pos = go.position, rot = go.rotation};
            string json = JsonUtility.ToJson(pr);
            Debug.Log(json);
            QuickSaveSettings settings = new QuickSaveSettings();
            settings.SecurityMode = SecurityMode.Base64;
            QuickSaveWriter writer = QuickSaveWriter.Create("TestRoot4", settings);
            writer.Write("pr", json);
            writer.Commit();

            QuickSaveReader reader = QuickSaveReader.Create("TestRoot4", settings);
            string j2 = reader.Read<string>("pr");
            PosRot pr2 = JsonUtility.FromJson<PosRot>(j2);
            pos = pr2.pos;
            rot = pr2.rot;*/
        }

        private void FixedUpdate()
        {
            if (saveB)
            {
                save();
                saveB = false;
            }
            
            if (loadB)
            {
                load();
                loadB = false;
            }
        }

        private void save()
        {
            Dictionary<string, PosRot> prs = new Dictionary<string, PosRot>();

            foreach (GameObject part in BodyParts.getParts())
            {
                string name = part.name;
                Transform transform = part.transform;
                PosRot pr = new PosRot {pos = transform.position, rot = transform.rotation};
                prs[name] = pr;
            }

            QuickSaveSettings settings = new QuickSaveSettings();
            //settings.SecurityMode = SecurityMode.Base64;
            QuickSaveWriter writer = QuickSaveWriter.Create("TestPosRot2", settings);


            foreach (KeyValuePair<string, PosRot> keyValuePair in prs)
            {
                writer.Write(keyValuePair.Key, JsonUtility.ToJson(keyValuePair.Value));
            }
            writer.Commit();
        }
        
        private void load()
        {
            Dictionary<string, PosRot> prs = new Dictionary<string, PosRot>();
            QuickSaveSettings settings = new QuickSaveSettings();
            
            //settings.SecurityMode = SecurityMode.Base64;
            QuickSaveReader reader = QuickSaveReader.Create("TestPosRot2", settings);

            foreach (string key in reader.GetAllKeys())
            {
                prs[key] = JsonUtility.FromJson<PosRot>(reader.Read<string>(key));
            }
            
            foreach (GameObject part in BodyParts.getParts())
            {
                
                
                string name = part.name;
                Transform transform = part.transform;
                if (prs.ContainsKey(name))
                {
                    Rigidbody rb = part.GetComponent<Rigidbody>();
                    if (rb!=null)
                    {
                        //rb.isKinematic = true;
                    }
                    transform.position = prs[name].pos;
                    transform.rotation = prs[name].rot;
                }
            }
        }
    }


    public class PosRot
    {
        public Vector3 pos;
        public Quaternion rot;

        public PosRot()
        {
        }
    }
}