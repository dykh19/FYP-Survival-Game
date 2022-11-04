using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(SkillsInteraction))]
public class SkillsUI : MonoBehaviour, IDragHandler, IBeginDragHandler
{
    private List<Node> nodes;
    private Transform container;
    private Vector2 lastPosition;
    private SkillsInteraction interaction;

    private const int iconSize = 100;
    private const int spacing = 200;
    private const int lineWidth = 10;
    private static readonly Color lineColor = Color.white;

    void Awake()
    {
        nodes = new List<Node>();
        container = transform.Find("Parent");

        interaction = gameObject.GetComponent<SkillsInteraction>();
    }
    
    void Start()
    {
        InitializeNodes();
        BuildUI(nodes, container);
        BuildLinks();
        FocusMiddle();
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        lastPosition = eventData.position;
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        var currentPosition = eventData.position;
        var displacement = currentPosition - lastPosition;

        container.position += new Vector3(displacement.x, displacement.y, 0);
        lastPosition = currentPosition;
    }

    private void InitializeNodes()
    {
        var skills = GameManager.Instance.PlayerSkills.skills;
        var nodes = skills.Select(skill => new Node(skill)).ToArray();

        foreach (var node in nodes)
        {
            var prerequisite = node.reference.skill.prerequisite;

            if (prerequisite.skill == null)
            {
                this.nodes.Add(node);
            }
            else
            {
                var prerequisiteNode = nodes.FirstOrDefault(
                    node => node.reference.skill.name == prerequisite.skill.name);

                prerequisiteNode.Add(node);
            }
        }
    }

    private void BuildUI(List<Node> nodes, Transform parent)
    {
        // Assume that every node UI in the given list is of width 'iconSize'.
        var widths = new float[nodes.Count];
        Array.Fill(widths, iconSize);

        // For each node in the given list:
        for (int i = 0; i < nodes.Count; i++)
        {
            var node = nodes[i];

            // Create a UI element.
            var icon = CreateRect(node.reference.skill.name, parent);
            icon.sizeDelta = new Vector2(iconSize, iconSize);

            // Set the skill icon image.
            var image = icon.gameObject.AddComponent<Image>();
            image.sprite = node.reference.skill.icon;
            image.color = (node.reference.level > 0) ? Color.white : Color.grey;
            node.reference.uiElement = image;

            // Add the UI functionality.
            var button = icon.gameObject.AddComponent<Button>();
            button.onClick.AddListener(() => interaction.DisplaySkill(node.reference));

            // If the node has child nodes:
            if (node.HasChildren)
            {
                // Create an empty wrapper object.
                var wrapper = CreateRect("Wrapper", icon);

                // Recurse for the node's child nodes under the wrapper object.
                BuildUI(node.nodes, wrapper);

                // Update the node UI's width value in the list.
                widths[i] = BoundaryChecker.GetWidth(wrapper);

                // Adjust the wrapper's position appropriately.
                var wrapperOffset = (-widths[i] / 2) + (iconSize / 4);
                wrapper.localPosition = new Vector3(wrapperOffset, -spacing, 0);
            }

            // Adjust the node UI's position appropriately.
            var offset = widths.Take(i).Sum() + (widths[i] / 2) + (i * spacing);
            icon.localPosition = new Vector3(offset, 0, 0);
        }
    }

    private void BuildLinks()
    {
        var linesLayer = CreateRect("Lines", container);
        linesLayer.SetAsFirstSibling();

        for (int i = 0; i < container.childCount; i++)
        {
            BuildLinks(container.GetChild(i), linesLayer);
        }
    }

    private void FocusMiddle()
    {
        int middle = container.childCount / 2;
        var midNode = container.GetChild(middle);
        container.localPosition = -midNode.localPosition;
    }

    private static RectTransform CreateRect(string name, Transform parent)
    {
        var element = new GameObject(name);
        var rect = element.AddComponent<RectTransform>();
        rect.SetParent(parent);

        return rect;
    }

    private static void BuildLinks(Transform parent, Transform container)
    {
        if (parent.childCount > 0)
        {
            var wrapper = parent.GetChild(0);
            for (int i = 0; i < wrapper.childCount; i++)
            {
                var child = wrapper.GetChild(i);
                DrawLine(parent.position, child.position, container);
                BuildLinks(child, container);
            }
        }
    }

    private static void DrawLine(Vector3 p1, Vector3 p2, Transform parent)
    {
        var midpoint = (p1 + p2) / 2;
        var distance = (p2 - p1).magnitude;

        var angle = Mathf.Atan2(p1.x - p2.x, p2.y - p1.y);
        if (angle < 0.0) angle += Mathf.PI * 2;
        angle *= Mathf.Rad2Deg;

        var rect = CreateRect("Line", parent);
        var rotation = Quaternion.Euler(0, 0, angle + 90);
        rect.sizeDelta = new Vector2(distance, lineWidth);
        rect.SetPositionAndRotation(midpoint, rotation);

        var image = rect.gameObject.AddComponent<Image>();
        image.color = lineColor;
    }

    private class Node
    {
        public PlayerSkill reference;
        public List<Node> nodes;
        public bool HasChildren { get { return nodes != null; } }

        public Node(PlayerSkill reference)
        {
            this.reference = reference;
        }

        public void Add(Node item)
        {
            if (nodes == null)
                nodes = new List<Node>();

            nodes.Add(item);
        }
    }

    private static class BoundaryChecker
    {
        private static readonly Vector3[] cornersArray = new Vector3[4];

        public static float GetLeftBoundary(RectTransform rect)
        {
            rect.GetWorldCorners(cornersArray);
            return cornersArray[0].x;
        }

        public static float GetRightBoundary(RectTransform rect)
        {
            rect.GetWorldCorners(cornersArray);
            return cornersArray[2].x;
        }

        public static float GetWidth(RectTransform rect)
        {
            float left = float.MaxValue;
            float right = float.MinValue;

            foreach (var child in rect.GetComponentsInChildren<RectTransform>())
            {
                left = Mathf.Min(left, GetLeftBoundary(child));
                right = Mathf.Max(right, GetRightBoundary(child));
            }

            return right - left;
        }
    }
}
