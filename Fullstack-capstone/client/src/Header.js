import React, { useState, useContext, useEffect } from 'react';
import { NavLink as RRNavLink } from "react-router-dom";
import {
  Collapse,
  Navbar,
  NavbarToggler,
  NavbarBrand,
  Nav,
  NavItem,
  NavLink
} from 'reactstrap';
import { UserProfileContext } from "./providers/UserProfileProvider";

export default function Header() {
  const { isLoggedIn, logout, activeUser, userTypeId } = useContext(UserProfileContext);
  const [isOpen, setIsOpen] = useState(false);
  const toggle = () => setIsOpen(!isOpen);

  const [refresh, setRefresh] = useState(false);

  return (
    <>
      <div>
        <Navbar color="light" light expand="md">
          <NavbarBrand tag={RRNavLink} to="/">Artorama</NavbarBrand>
          <NavbarToggler onClick={toggle} />
          <Collapse isOpen={isOpen} navbar>
            <Nav className="mr-auto" navbar>
              { /* When isLoggedIn === true, we will render the Home link */}
              {isLoggedIn &&
                <NavItem>
                  <NavLink tag={RRNavLink} to="/">Home</NavLink>
                </NavItem>
              }

              {isLoggedIn &&
                <NavItem>
                  <NavLink tag={RRNavLink} to="/post">Posts</NavLink>
                </NavItem>
              }



            </Nav>

            <Nav navbar>
              {isLoggedIn &&
                <>
                  <NavItem>
                    <a aria-current="page" className="nav-link"
                      style={{ cursor: "pointer" }} onClick={logout}>Logout</a>
                  </NavItem>
                </>
              }
              {!isLoggedIn &&
                <>
                  <NavItem>
                    <NavLink tag={RRNavLink} to="/login">Login</NavLink>
                  </NavItem>
                  <NavItem>
                    <NavLink tag={RRNavLink} to="/register">Register</NavLink>
                  </NavItem>
                </>
              }
            </Nav>
          </Collapse>
        </Navbar>
      </div>
    </>
  );

}
