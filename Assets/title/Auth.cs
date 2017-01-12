using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;
using System;
using UnityEngine.SceneManagement;

public class Auth : MonoBehaviour {
    Firebase.Auth.FirebaseAuth auth;
    Firebase.Auth.FirebaseUser user;
    public InputField inputFieldEmail;
    public InputField inputFieldPassword;

    // initialization
    void InitializeFirebase()
    {
        //DebugLog("Setting up Firebase Auth");
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
    }

    // Track state changes of the auth object.
    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != user)
        {
            if (user == null && auth.CurrentUser != null)
            {
                //DebugLog("Signed in " + auth.CurrentUser.DisplayName);
            }
            else if (user != null && auth.CurrentUser == null)
            {
                //DebugLog("Signed out " + user.DisplayName);
            }
            user = auth.CurrentUser;
        }
    }

    public void SignIn()
    {
        String email = inputFieldEmail.text.ToString();
        String password = inputFieldPassword.text.ToString();
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(
      task => {
          if (!task.IsCanceled && !task.IsFaulted)
          {
              // User has been created.
          }
          else
          {
              // User creation has failed.
          }
      });
    }
    public void SignUp()
    {
        String email = inputFieldEmail.text.ToString();
        String password = inputFieldPassword.text.ToString();
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(
      task => {
          if (!task.IsCanceled && !task.IsFaulted)
          {
              // User has been created.
          }
          else
          {
              // User creation has failed.
          }
      });
    }
    // cleanup
    void OnDestroy()
    {
        auth.StateChanged -= AuthStateChanged;
        auth = null;
    }



    // Use this for initialization
    void Start () {
        InitializeFirebase();
    }
	
	// Update is called once per frame
	void Update () {
		
	}



}
