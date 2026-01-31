
    public interface IDraggable
    {
        public bool DragIsEnabled { get; set; }
        public void EnableDragging(bool enable);
        public void OnMouseDown();
        public void OnMouseDrag();
        public void OnMouseUp();
    }