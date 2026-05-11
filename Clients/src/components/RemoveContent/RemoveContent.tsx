import {deleteusercontent} from "../../services/userContentService";
import "./RemoveContent.css";

export interface RemoveContentProps {
    id: number;
    onClose: () => void;
    onRemove?: () => void;
}

export function RemoveContent({ id, onClose, onRemove }: RemoveContentProps) {
    return (
        <div className="modal-overlay">
            <div className="modal-content">
                <button className="close-button" onClick={onClose}>×</button>
                <h2>Xác nhận xóa nội dung chi tiêu</h2>
                <p>Bạn có chắc chắn muốn xóa nội dung chi tiêu này không?</p>
                <div className="modal-actions">
                    <button onClick={onClose}>Hủy</button>
                    <button onClick={async () => {
                        await deleteusercontent(id);
                        onRemove?.();
                        onClose();
                    }}>Xóa</button>
                </div>
            </div>
        </div>
    );
}