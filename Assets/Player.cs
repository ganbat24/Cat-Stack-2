using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] List<Transform> selectionPoints;
    [SerializeField] List<TetrisCat> cats;
    List<TetrisCat> current = new List<TetrisCat>();

    private void Start() {
        current.Clear();
        for(int i = 0; i < selectionPoints.Count; i++){
            TetrisCat cat = Instantiate(randomCat(), selectionPoints[i]);
            cat.OnFreedom += ReInstance(i); 
            current.Add(cat);
        }
    }

    private System.Action ReInstance(int index){
        return () => {
            current[index] = Instantiate(randomCat(), selectionPoints[index]);
            current[index].OnFreedom += ReInstance(index);
        };
    }

    private TetrisCat randomCat(){
        return cats[Random.Range(0, cats.Count)];
    }
}
