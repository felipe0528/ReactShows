import React from 'react'
import { Container, Menu } from 'semantic-ui-react'

export const Navbar = () => {
    return (
        <Menu fixed='top' inverted>
            <Container>
                <Menu.Item header  to='/'>
                    ReactShows
                </Menu.Item>
                {/* {user && (
                    <Menu.Item position='right'>
                    <Dropdown pointing='top left' text={user.displayName}>
                        <Dropdown.Menu>
                        <Dropdown.Item
                            as={Link}
                            to={`/profile/${user.username}`}
                            text='My profile'
                            icon='user'
                        />
                        <Dropdown.Item onClick={logout} text='Logout' icon='power' />
                        </Dropdown.Menu>
                    </Dropdown>
                    </Menu.Item>
                )} */}
            </Container>
        </Menu>
    )
}
