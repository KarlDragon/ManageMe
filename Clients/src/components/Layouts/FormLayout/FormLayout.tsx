import React from "react";
import './FormLayout.css'; // optional separate CSS

export const FormLayout: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  return (
    <div className="app">
      <main>
        <div className="formContainer">
          <div className="formCard">
            {children}
          </div>
        </div>
      </main>
    </div>
  );
};
