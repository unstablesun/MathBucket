using UnityEngine;
using System.Collections;


public static class SceneService
{
    public static void LoadScene<T>( System.Action<T> callback )
        where T : SceneController
    {
        // Disable interaction on everything so EventSystem doesn't whine
        foreach( SceneController controller in SceneRepository.GetAllScenes() )
            controller.SetInteractable( false );
                
        // Load the new scene
        SceneRepository.LoadScene<T>( true, true, delegate( T loadedSceneController )
        {
            // Unload everything but the scene we just loaded
            SceneRepository.UnloadScenes( delegate( SceneController controller )
            {
                return (controller != loadedSceneController);
            });

            // Cleanup assets
            Resources.UnloadUnusedAssets();
    
            // Enable interaction for the loaded scene
            loadedSceneController.SetInteractable( true );

            // Notify the caller
            if( callback != null )
                callback( loadedSceneController );
        });
    }

    public static void LoadScene<T,LoadingScreen>( System.Action<T> callback )
        where T : SceneController
        where LoadingScreen : SceneController
    {
        // Disable interaction on everything
        foreach( SceneController controller in SceneRepository.GetAllScenes() )
            controller.SetInteractable( false );

        // Show the loading screen (and load it on-the-fly if necessary)
        SceneRepository.LoadScene<LoadingScreen>( true, false, delegate( LoadingScreen loadingScreen )
        {
            loadingScreen.SetVisible( true, true, delegate()
            {
                // Unload everything but the loading screen
                SceneRepository.UnloadScenes( delegate( SceneController controller )
                {
                    return (controller != loadingScreen);
                });
                
                // Load the new scene
                SceneRepository.LoadScene<T>( true, true, delegate( T loadedSceneController )
                {
                    // Cleanup assets
                    Resources.UnloadUnusedAssets();
            
                    // Hide the loading screen
                    loadingScreen.SetVisible( false, true, delegate()
                    {
                        // Enable interaction for the loaded scene
                        loadedSceneController.SetInteractable( true );

                        // Notify the caller
                        if( callback != null )
                            callback( loadedSceneController );
                    });
                });
            });
        });
    }
}

