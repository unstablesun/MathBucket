using UnityEngine;
using System.Collections;



public abstract class PopupController : SplashScreenController
{
    [SerializeField]
    public Transform popup;

    private System.Action<int> onDismissCallback;

	protected override void BackButton () {
		Dismiss ();
	}

    // Prevent causal unloading of popup scenes
    public override bool ShouldUnload()
    {
        return false;
    }

    // Override the "alpha" 
    protected override void SetAlpha( float alpha )
    {
        base.SetAlpha( alpha );

        if( popup )
        {
            float s = Mathf.Lerp( 0.25f, 1.0f, alpha );
            popup.localScale = new Vector3( s, s, 1.0f );
        }
    }

    // Present
    public void Present()
    {
        Present( true, null );
    }

    public void Present( System.Action<int> _onDismissCallback )
    {
        Present( true, _onDismissCallback );
    }
    
    public void Present( bool animated, System.Action<int> _onDismissCallback )
    {
        onDismissCallback = _onDismissCallback;
        
        SetVisible( true, animated, delegate()
        {
            SetInteractable( true );
        });
    }

    // Dismiss
    public void Dismiss()
    {
        Dismiss( true, 0 );
    }
    
    public void Dismiss( int result )
    {
        Dismiss( true, result );
    }
    
    public void Dismiss( bool animated, int result )
    {
        SetInteractable( false );
        
        if( onDismissCallback != null )
        {
            System.Action<int> callback = onDismissCallback;
            onDismissCallback = null;

            SetVisible( false, animated, delegate()
            {
                callback( result );
            });
        }
        
        else
        {
            SetVisible( false, animated, null );
        }
    }
}


