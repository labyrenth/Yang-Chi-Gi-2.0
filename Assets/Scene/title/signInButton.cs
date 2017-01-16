using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using Firebase.Auth;

public class signInButton : MonoBehaviour {

    public InputField inputEmail;
    public InputField inputPassword;

    FirebaseDatabase database;
    DatabaseReference emailReference;
    FirebaseAuth auth;
    FirebaseUser user;
	public void signIn()
    {
        var email = inputEmail.text.ToString();
        var password = inputPassword.text.ToString();
        if (auth.SignInWithEmailAndPasswordAsync(email, password).IsCompleted)
        {
            databaseCheck(email);
        }
    } 
    
    public void databaseCheck(string email)
    {
        emailReference.Child(user.UserId).SetValueAsync(email);
    }

    private void Start()
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://yang-chigi.firebaseio.com/");
        this.database = FirebaseDatabase.DefaultInstance;
        this.emailReference = database.GetReference("user");
        this.auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        this.gameObject.GetComponent<Button>().onClick.AddListener(signIn);
        
    }

    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != user)
        {
            user = auth.CurrentUser;
        }
    }

    void OnDestroy()
    {
        auth.StateChanged -= AuthStateChanged;
        auth = null;
    }

}
