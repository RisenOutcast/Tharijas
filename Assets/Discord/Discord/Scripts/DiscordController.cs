using UnityEngine;

[System.Serializable]
public class DiscordJoinEvent : UnityEngine.Events.UnityEvent<string> { }

[System.Serializable]
public class DiscordSpectateEvent : UnityEngine.Events.UnityEvent<string> { }

[System.Serializable]
public class DiscordJoinRequestEvent : UnityEngine.Events.UnityEvent<DiscordRpc.JoinRequest> { }

public class DiscordController : MonoBehaviour
{
    public DiscordRpc.RichPresence presence = new DiscordRpc.RichPresence();
    public string applicationId;
    public string optionalSteamId;
    public int callbackCalls;
    public int clickCounter;
    public DiscordRpc.JoinRequest joinRequest;
    public UnityEngine.Events.UnityEvent onConnect;
    public UnityEngine.Events.UnityEvent onDisconnect;
    public UnityEngine.Events.UnityEvent hasResponded;
    public DiscordJoinEvent onJoin;
    public DiscordJoinEvent onSpectate;
    public DiscordJoinRequestEvent onJoinRequest;

    string playingGrontto = "grontto";
    string playingAngira = "angira";
    string playingAs;

    string fightingWith;

    DiscordRpc.EventHandlers handlers;

    public void Alotus()
    {
        Debug.Log("Discord: Started!");

        presence.details = string.Format("1v1");
        presence.state = string.Format("Normal", 128);
        presence.largeImageKey = string.Format("battle", 32);
        presence.largeImageText = string.Format("Tharijas", 128);
        presence.smallImageKey = string.Format(playingAs, 32);
        presence.smallImageText = string.Format(fightingWith, 128);

        DiscordRpc.UpdatePresence(presence);
    }

    public void Menu()
    {
        Debug.Log("Discord: Started!");

        presence.details = string.Format("In Menus");
        presence.state = string.Format("MainMenu", 128);
        presence.largeImageKey = string.Format("logo", 32);
        presence.largeImageText = string.Format("Tharijas", 128);


        DiscordRpc.UpdatePresence(presence);
    }

    public void LoginScreen()
    {
        Debug.Log("Discord: Started!");

        presence.details = string.Format("In Menus");
        presence.state = string.Format("Lobby", 128);
        presence.largeImageKey = string.Format("logo", 32);
        presence.largeImageText = string.Format("Tharijas", 128);


        DiscordRpc.UpdatePresence(presence);
    }

    public void RequestRespondYes()
    {
        Debug.Log("Discord: responding yes to Ask to Join request");
        DiscordRpc.Respond(joinRequest.userId, DiscordRpc.Reply.Yes);
        hasResponded.Invoke();
    }

    public void RequestRespondNo()
    {
        Debug.Log("Discord: responding no to Ask to Join request");
        DiscordRpc.Respond(joinRequest.userId, DiscordRpc.Reply.No);
        hasResponded.Invoke();
    }

    public void ReadyCallback()
    {
        ++callbackCalls;
        Debug.Log("Discord: ready");
        onConnect.Invoke();
    }

    public void DisconnectedCallback(int errorCode, string message)
    {
        ++callbackCalls;
        Debug.Log(string.Format("Discord: disconnect {0}: {1}", errorCode, message));
        onDisconnect.Invoke();
    }

    public void ErrorCallback(int errorCode, string message)
    {
        ++callbackCalls;
        Debug.Log(string.Format("Discord: error {0}: {1}", errorCode, message));
    }

    public void JoinCallback(string secret)
    {
        ++callbackCalls;
        Debug.Log(string.Format("Discord: join ({0})", secret));
        onJoin.Invoke(secret);
    }

    public void SpectateCallback(string secret)
    {
        ++callbackCalls;
        Debug.Log(string.Format("Discord: spectate ({0})", secret));
        onSpectate.Invoke(secret);
    }

    public void RequestCallback(ref DiscordRpc.JoinRequest request)
    {
        ++callbackCalls;
        Debug.Log(string.Format("Discord: join request {0}#{1}: {2}", request.username, request.discriminator, request.userId));
        joinRequest = request;
        onJoinRequest.Invoke(request);
    }

    void Start()
    {
    }

    void Update()
    {
        DiscordRpc.RunCallbacks();

        if (RO.Settings.peliSäätäjä.all_players[0].Angira == true)
        {
            playingAs = playingAngira;
            fightingWith = "Fighting with Angira";
        }

        if (RO.Settings.peliSäätäjä.all_players[0].Grontto == true)
        {
            playingAs = playingGrontto;
            fightingWith = "Fighting with Grontto";
        }

    }

    void OnEnable()
    {
        Debug.Log("Discord: init");
        callbackCalls = 0;

        handlers = new DiscordRpc.EventHandlers();
        handlers.readyCallback = ReadyCallback;
        handlers.disconnectedCallback += DisconnectedCallback;
        handlers.errorCallback += ErrorCallback;
        handlers.joinCallback += JoinCallback;
        handlers.spectateCallback += SpectateCallback;
        handlers.requestCallback += RequestCallback;
        DiscordRpc.Initialize(applicationId, ref handlers, true, optionalSteamId);
    }

    public void OnDisable()
    {
        Debug.Log("Discord: shutdown");
        DiscordRpc.ClearPresence();
        DiscordRpc.Shutdown();
    }

    public void ClearPresence()
    {
        Debug.Log("Discord: Cleared Precence!");
        DiscordRpc.ClearPresence();
    }

    void OnDestroy()
    {

    }
}
