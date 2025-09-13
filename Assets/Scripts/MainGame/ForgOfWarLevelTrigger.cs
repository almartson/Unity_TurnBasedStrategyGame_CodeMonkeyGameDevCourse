/* NOTE: Modified Unity C# Script Template by Alec AlMartson...
...on Path:   /PathToUnityHub/Unity/Hub/Editor/UNITY_VERSION_FOR_EXAMPLE__2020.3.36f1/Editor/Data/Resources/ScriptTemplates/81-C# Script-NewBehaviourScript.cs
*/

using System;
using System.Collections.Generic;
using UnityEngine;


public class ForgOfWarLevelTrigger : MonoBehaviour
{

    #region Attributes

    [Tooltip("Tag To Verify -> in the collisions.")]
    private const string _COLLIDER_TAG = "Player";
    
    [Tooltip("It Was Triggered Already [Readonly] -> True: if this Script has been already triggered.")]
    [SerializeField]
    private bool _itWasTriggeredAlready = false;

    
    [Tooltip("Fog Of War GameObjects List -> List of Black (colored) Quads that will disappear once the Trigger has been activated.")]
    [SerializeField]
    private List<GameObject> _fogOfWarGameObjectsList = new List<GameObject>();

    
    [Tooltip("Enemies GameObject List -> List of Enemies that will appear in the Room, once the Trigger has been activated.")]
    [SerializeField]
    private List<GameObject> _enemiesGameObjectsList = new List<GameObject>();

    
    #endregion Attributes


    #region Unity Methods

    /// <summary>
    /// Awake is called before the Start calls round
    /// </summary>



    /// <summary>
    /// Start is called before the first frame update
    /// </summary>




    private void OnTriggerEnter(Collider other)
    {
        
        // Check that the collision was with an Unit that belongs to the "Player":
        //
        if (other.gameObject.CompareTag(  _COLLIDER_TAG ) && (! _itWasTriggeredAlready))
        {
            
            // Debug.Log($"Triggered by the Player! with -> {other.gameObject}");

            // 0- Mark that this Trigger is been used now:
            //
            _itWasTriggeredAlready = true;
            
            // 1- Hide the FOG OF WAR:
            //
            HideFogOfWarGameObjects();
            
            // 2- Enable (show) the enemies in the area.
            //
            ShowEnemiesGameObjects();           
            
        }//End if (other.gameObject.CompareTag(  _COLLIDER_TAG )..
        
    }//End OnTriggerEnter

    #endregion Unity Methods
    

    #region My Custom Methods
    
    /// <summary>
    /// Hides the FOG OF WAR  (GameObjects)
    /// </summary>
    public void HideFogOfWarGameObjects()
    {
        
        int fogOfWarGameObjectsListLength = _fogOfWarGameObjectsList.Count;
        //
        for (int i = 0; i < fogOfWarGameObjectsListLength; i++)
        {
            _fogOfWarGameObjectsList[i].SetActive(false);
        }
        
    }//End HideFogOfWarGameObjects
    
    /// <summary>
    /// Shows the FOG OF WAR  (GameObjects)
    /// </summary>
    public void ShowFogOfWarGameObjects()
    {
        
        int fogOfWarGameObjectsListLength = _fogOfWarGameObjectsList.Count;
        //
        for (int i = 0; i < fogOfWarGameObjectsListLength; i++)
        {
            _fogOfWarGameObjectsList[i].SetActive(true);
        }
        
    }//End ShowFogOfWarGameObjects
    
    
    /// <summary>
    /// Shows the ENEMIES in the area  (GameObjects)
    /// </summary>
    public void ShowEnemiesGameObjects()
    {
        
        int arrayLength = _enemiesGameObjectsList.Count;
        //
        for (int i = 0; i < arrayLength; i++)
        {
            _enemiesGameObjectsList[i].SetActive(true);
        }
        
    }//End ShowFogOfWarGameObjects

    /// <summary>
    /// Hide the ENEMIES in the area  (GameObjects)
    /// </summary>
    public void HideEnemiesGameObjects()
    {
        
        int arrayLength = _enemiesGameObjectsList.Count;
        //
        for (int i = 0; i < arrayLength; i++)
        {
            _enemiesGameObjectsList[i].SetActive(false);
        }
        
    }//End HideEnemiesGameObjects
    

    #endregion My Custom Methods

}
