using UnityEngine;
using System.Collections;
using System.Collections.Generic;


// SceneRepository Service ===============================================================
public class SceneRepository
{
    // Predicates ------------------------------------------------------------------------
    public delegate bool Predicate( SceneController controller );
    
    public static bool MatchAll( SceneController controller )
    {
        return true;
    }

    // Scene Registry --------------------------------------------------------------------
    public static void RegisterScene<T>() where T : SceneController
    {
        SceneRepositoryImp.SharedObject.RegisterScene( typeof(T) );
    }

    public static ICollection<System.Type> GetRegisteredSceneTypes()
    {
        return SceneRepositoryImp.SharedObject.GetRegisteredSceneTypes();
    }

    // Scene Lookup ----------------------------------------------------------------------
    public static T GetScene<T>() where T : SceneController
    {
        return SceneRepositoryImp.SharedObject.GetScene( typeof(T) ) as T;
    }
    
    public static List<SceneController> GetScenes( Predicate predicate )
    {
        return SceneRepositoryImp.SharedObject.GetScenes( predicate );
    }
    
    public static List<SceneController> GetAllScenes()
    {
        return SceneRepositoryImp.SharedObject.GetScenes( MatchAll );
    }

    // Scene Lookup+Loading --------------------------------------------------------------
    public static void LoadScene<T>( bool additive, bool async, System.Action<T> callback ) where T : SceneController
    {
        SceneRepositoryImp.SharedObject.LoadScene( typeof(T), additive, async, delegate( SceneController controller )
        {
            if( callback != null )
                callback( controller as T );
        });
    }
    
    public static void UnloadScene<T>() where T : SceneController
    {
        SceneRepositoryImp.SharedObject.UnloadScene( typeof(T) );
    }

    public static void UnloadScenes( Predicate predicate )
    {
        SceneRepositoryImp.SharedObject.UnloadScenes( predicate );
    }

    public static void UnloadAllScenes()
    {
        SceneRepositoryImp.SharedObject.UnloadScenes( MatchAll );
    }
}



// SceneRepository Implementation ========================================================
public class SceneRepositoryImp : MonoBehaviour
{
    // SharedObject ----------------------------------------------------------------------
    private static SceneRepositoryImp _sharedObject;
    
    public static SceneRepositoryImp SharedObject
    {
        get
        {
            if( _sharedObject == null )
            {
                GameObject gameObject = new GameObject( "SceneRepository" );
                GameObject.DontDestroyOnLoad( gameObject );
                
                _sharedObject = gameObject.AddComponent<SceneRepositoryImp>();
            }
            
            return _sharedObject;
        }
    }
    
    
    // Scene Registry --------------------------------------------------------------------
    class SceneInfo
    {
        public readonly string name;
        public SceneController controller;
        public bool loading;
        
        public SceneInfo( string _name )
        {
            name = _name;
            controller = null;
            loading = false;
        }
    }
    
    private Dictionary<System.Type,SceneInfo> scenes = new Dictionary<System.Type,SceneInfo>();
    
    public void RegisterScene( System.Type controllerType )
    {
        if( !scenes.ContainsKey( controllerType ) )
        {
            string name = SceneController.GetSceneFileName( controllerType );
            scenes.Add( controllerType, new SceneInfo( name ) );
        }
    }

    public void SetSceneControllerReference( SceneController controller )
    {
        RegisterScene( controller.GetType() );
    
        SceneInfo sceneInfo;
        
        if( scenes.TryGetValue( controller.GetType(), out sceneInfo ) )
        {
            if( (sceneInfo.controller != null) && (sceneInfo.controller != controller) )
                Debug.LogError( string.Format( "SceneRepository: Trying to register multiple instances of {0}", controller.GetType().Name ) );
        
            sceneInfo.controller = controller;
        }
    }
    
    public ICollection<System.Type> GetRegisteredSceneTypes()
    {
        return scenes.Keys;
    }
    
    
    // Scene Lookup ----------------------------------------------------------------------
    public SceneController GetScene( System.Type sceneControllerType )
    {
        SceneInfo sceneInfo;
        
        if( scenes.TryGetValue( sceneControllerType, out sceneInfo ) )
            return sceneInfo.controller;
        
        return null;
    }

    public List<SceneController> GetScenes( SceneRepository.Predicate predicate )
    {
        List<SceneController> list = new List<SceneController>();
        
        foreach( KeyValuePair<System.Type,SceneInfo> entry in scenes )
        {
            if( entry.Value.controller )
            {
                if( predicate( entry.Value.controller ) )
                    list.Add( entry.Value.controller );
            }
        }
        
        return list;
    }

    
    // Scene Lookup+Loading --------------------------------------------------------------
    public void LoadScene( System.Type sceneControllerType, bool additive, bool async, System.Action<SceneController> callback )
    {
        RegisterScene( sceneControllerType );

        SceneInfo sceneInfo;
        
        if( scenes.TryGetValue( sceneControllerType, out sceneInfo ) )
        {
            if( !sceneInfo.controller )
            {
                StartCoroutine( LoadScene( sceneInfo, additive, async, callback ) );
            }

            else
            {
                if( callback != null )
                    callback( sceneInfo.controller );
            }
        }

        else
        {
            if( callback != null )
                callback( sceneInfo.controller );
        }
    }

    private IEnumerator LoadScene( SceneInfo sceneInfo, bool additive, bool async, System.Action<SceneController> callback )
    {
        // Already loading?
        while( sceneInfo.loading )
            yield return null;
            
        if( !sceneInfo.controller )
        {
            sceneInfo.loading = true;
            
            if( additive )
            {
                if( async )
                {
                    yield return UnityEngine.Application.LoadLevelAdditiveAsync( sceneInfo.name );
                }
        
                else
                {
                    // Note: LoadLevelAdditive does not fully complete until next frame
                    UnityEngine.Application.LoadLevelAdditive( sceneInfo.name );
                    yield return null;
                }
            }
        
            else
            {
                if( async )
                {
                    yield return UnityEngine.Application.LoadLevelAsync( sceneInfo.name );
                }
        
                else
                {
                    // Note: LoadLevel does not fully complete until next frame
                    UnityEngine.Application.LoadLevel( sceneInfo.name );
                    yield return null;
                }
            }

            sceneInfo.loading = false;
        }

        if( callback != null )
            callback( sceneInfo.controller );
    }

    public void UnloadScene( System.Type sceneControllerType )
    {
        SceneInfo sceneInfo;
        
        if( scenes.TryGetValue( sceneControllerType, out sceneInfo ) )
        {
            if( sceneInfo.controller )
            {
                if( !sceneInfo.controller.ShouldUnload() )
                {
                    Debug.LogWarning( string.Format( "Unloading Scene {0}",
                        SceneController.GetSceneFileName( sceneControllerType ) ) );
                }
            
                GameObject.Destroy( sceneInfo.controller.gameObject );
                sceneInfo.controller = null;
            }
        }
    }

    public void UnloadScenes( SceneRepository.Predicate predicate )
    {
        foreach( KeyValuePair<System.Type,SceneInfo> entry in scenes )
        {
            if( entry.Value.controller )
            {
                if( entry.Value.controller.ShouldUnload() )
                {
                    if( predicate( entry.Value.controller ) )
                    {
                        GameObject.Destroy( entry.Value.controller.gameObject );
                        entry.Value.controller = null;
                    }
                }
            }
        }
    }
}
