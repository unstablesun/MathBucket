using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Reflection;

public abstract class SceneController : MonoBehaviour
{
    // Scene File Name Reflection ========================================================
    
    // *** OVERRIDE THIS PROPERTY IN EVERY SUBCLASS ***
    private static string SceneFileName { get { return null; } }
    

    public static string GetSceneFileName<T>() where T : SceneController
    {
        return GetSceneFileName( typeof(T) );
    }
    
    public static string GetSceneFileName( System.Type controllerType )
    {
        PropertyInfo propertyInfo = controllerType.GetProperty( "SceneFileName",
            BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.FlattenHierarchy );

        if( propertyInfo != null )
        {
            string sceneFileName = propertyInfo.GetValue( null, null ) as string;
            
            if( sceneFileName != null )
                return sceneFileName;
        }

        string error = string.Format( "SceneController subclass '{0}' does not define a 'SceneFileName' property.", controllerType.FullName );
        throw new System.Exception( error );
    }    
    

    // Variables =========================================================================
    [SerializeField]
    protected Camera[] cameras;

    [SerializeField]
    protected Canvas[] canvases;

    [SerializeField]
    protected CanvasGroup[] canvasGroups;

    [SerializeField]
    protected EventSystem eventSystem;
    
    [SerializeField][HideInInspector]
    private bool _visible = true;

    [SerializeField][HideInInspector]
    private bool _interactable = true;


    // MonoBehaviour =====================================================================
    protected virtual void Awake()
    {
        // This needs to happen here (instead of inside SceneRepository) to properly
        // handle loading the first scene, or any other case that doesn't go through the
        // repository.
        SceneRepositoryImp.SharedObject.SetSceneControllerReference( this );

        // Syncronize visible/interactive state
        ApplyVisible();
        ApplyInteractable();
    }

	protected virtual void Update() {
		if ( Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Escape) ) { 
			if( interactable ) {
				BackButton ();
			}
		}
	}

    // Interface =========================================================================

	protected virtual void BackButton () {
		SetInteractable( false );
		/*
		SceneRepository.LoadScene<NotificationPopupController>( true, false, delegate( NotificationPopupController popupController ){
			popupController.Init(
				"Exit Game",
				"Are you sure to exit the game?",
				"Yes",
				() => {
					UnityEngine.Application.Quit ();
				},
				"No",
				null
			);
			popupController.Present( delegate( int result ){
				Destroy(popupController.gameObject);
				SetInteractable( true );
			});
		});
		*/
	}

    public virtual bool ShouldUnload()
    {
        return true;
    }
    
    public void AutoConfigure()
    {
        cameras = gameObject.GetComponentsInChildren<Camera>();
        canvases = gameObject.GetComponentsInChildren<Canvas>();
        canvasGroups = gameObject.GetComponentsInChildren<CanvasGroup>();
        eventSystem = gameObject.GetComponentInChildren<EventSystem>();
    }
    
    protected void ApplyVisible()
    {
        if( cameras != null )
        {
            for( int i = 0; i < cameras.Length; i++ )
            {
                if( cameras[i] )
                    cameras[i].enabled = visible;
            }
        }

        if( canvases != null )
        {
            for( int i = 0; i < canvases.Length; i++ )
            {
                if( canvases[i] )
                {
                    canvases[i].enabled = visible;
                    
                    Camera worldCamera = canvases[i].worldCamera;
                    
                    if( worldCamera )
                        worldCamera.enabled = visible;
                }
            }
        }
    }
    
    protected void ApplyInteractable()
    {
        if( canvasGroups != null )
        {
            for( int i = 0; i < canvasGroups.Length; i++ )
            {
                if( canvasGroups[i] )
                    canvasGroups[i].interactable = visible && interactable;
            }
        }
            
        if( eventSystem )
        {
            eventSystem.enabled = visible && interactable;
        }
    }
    
    public virtual void SetVisible( bool value )
    {
        _visible = value;
        
        ApplyVisible();
        ApplyInteractable();
    }
    
    public virtual void SetVisible( bool value, bool animated, System.Action callback )
    {
        SetVisible( value );
        
        if( callback != null )
            callback();
    }

    public virtual void SetInteractable( bool value )
    {
        _interactable = value;
        
        ApplyInteractable();
    }


    // Wrappers ==========================================================================
    public bool visible
    {
        get { return _visible; }
        set { SetVisible( value ); }
    }
    
    public bool interactable
    {
        get { return _interactable; }
        set { SetInteractable( value ); }
    }
    
    public void Show()
    {
        SetVisible( true );
    }
    
    public void Hide()
    {
        SetVisible( false );
    }
}

