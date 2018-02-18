using Common;
using ProjectKillersCommon;
using ProjectKillersCommon.Data;
using ProjectKillersCommon.Data.Missions;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CreateRoomPanel : MonoBehaviour {
    [SerializeField] private TMP_InputField roomNameInput;
    [SerializeField] private TMP_Dropdown missionDropDown;

    private void OnEnable() {
        roomNameInput.text = string.Empty;

        List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();
        List<Type> types = BaseMission.GetMissionTypes();

        missionDropDown.ClearOptions();

        foreach (Type t in types) {
            options.Add(new TMP_Dropdown.OptionData(t.Name.ToString()));
        }

        missionDropDown.AddOptions(options);
    }

    public void CreateRoom() {
        Room room = new Room(roomNameInput.text, NetManager.I.ID);
        room.MissionName = missionDropDown.options[missionDropDown.value].text;

        NetManager.I.Client.SendRequest(Utils.ToBytesJSON(new NetData(RequestTypes.CreateRoom, new Dictionary<string, ObjectWrapper>() { { "room", new ObjectWrapper<Room>(room) } })));
        gameObject.SetActive(false);
    }
}
